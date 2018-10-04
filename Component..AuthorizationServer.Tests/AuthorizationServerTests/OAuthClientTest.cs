using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Xunit;

namespace Component.AuthorizationServer.Tests
{
    public class OAuthClientTest
    {
        private const string HOST_ADDRESS = "http://localhost:31338";
        private const string AuthorizationCodeRedirectURL = "http://localhost:31338/api/AuthorizationCode";
        private const string ImplicitGrantRedirectURL = "http://localhost:31338/api/ImplicitToken";
        private const string Client = "Client";
        private static HttpClient _httpClient;

        public OAuthClientTest()
        {
            Console.WriteLine("Web API started!");
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(HOST_ADDRESS);
            Console.WriteLine("HttpClient started!");
        }

        private static async Task<TokenResponse> GetToken(string grantType, string refreshToken = null, string userName = null, string password = null, string authorizationCode = null)
        {
            var clientId = "ClientId";
            var clientSecret = "clientSecret";
            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", grantType);

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                parameters.Add("username", userName);
                parameters.Add("password", password);
            }
            if (!string.IsNullOrEmpty(authorizationCode))
            {
                parameters.Add("code", authorizationCode);
                parameters.Add("redirect_uri", AuthorizationCodeRedirectURL); //和获取 authorization_code 的 redirect_uri 必须一致，不然会报错
            }
            if (!string.IsNullOrEmpty(refreshToken))
            {
                parameters.Add("refresh_token", refreshToken);
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret)));

            var httpContent= new FormUrlEncodedContent(parameters);
            
            var response = await _httpClient.PostAsync("/token", httpContent);
            var responseValue = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.StatusCode);
                //Console.WriteLine((await response.Content.ReadAsAsync<HttpError>()).Message);
                return null;
            }
            var tokenResponse = await response.Content.ReadAsAsync<TokenResponse>();
            return tokenResponse;
        }

        private static async Task<string> GetAuthorizationCode()
        {
            var clientId = "ClientId";

            var redirect_uri = HttpUtility.UrlEncode(AuthorizationCodeRedirectURL);
            var response = await _httpClient.GetAsync($"/authorize?grant_type=authorization_code&response_type=code&client_id={clientId}&redirect_uri={redirect_uri}");
            var authorizationCode =await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.StatusCode);
                //Console.WriteLine((await response.Content.ReadAsAsync<HttpError>()).Message);
                return null;
            }
            return authorizationCode;
        }

        [Fact]
        public async Task OAuthAuthorizationCodeAsyncTest()
        {
            var authorizationCode = GetAuthorizationCode().Result; //获取 authorization_code
            var tokenResponse =GetToken("authorization_code", null, null, null, authorizationCode).Result; //根据 authorization_code 获取 access_token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

            var response = await _httpClient.GetAsync($"/api/values");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.StatusCode);
                //Console.WriteLine((await response.Content.ReadAsAsync<HttpError>()).Message);
            }
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Xunit.Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Thread.Sleep(10000);

            var tokenResponseTwo = GetToken("refresh_token", tokenResponse.RefreshToken).Result; //根据 refresh_token 获取 access_token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponseTwo.AccessToken);
            var responseTwo = await _httpClient.GetAsync($"/api/values");
            Xunit.Assert.Equal(HttpStatusCode.OK, responseTwo.StatusCode);
        }

        [Fact]
        public async Task OAuthImplicitAsyncTest()
        {
            var clientId = Client;

            var tokenResponse = await _httpClient.GetAsync($"/authorize?response_type=token&client_id={clientId}&redirect_uri={ImplicitGrantRedirectURL}");
            //redirect_uri: http://localhost:8001/api/access_token#access_token=AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAfoPB4HZ0PUe-X6h0UUs2q42&token_type=bearer&expires_in=10           
            var accessToken = "";//get form redirect_uri
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"/api/values");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.StatusCode);
                //Console.WriteLine((await response.Content.ReadAsAsync<HttpError>()).Message);
            }
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task OAuthPasswordTest()
        {
            var tokenResponse = GetToken("password", null, "Client", "Password").Result; //获取 access_token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

            var response = await _httpClient.GetAsync($"/api/values");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.StatusCode);
                //Console.WriteLine((await response.Content.ReadAsAsync<HttpError>()).Message);
            }
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Thread.Sleep(10000);

            var tokenResponseTwo = GetToken("refresh_token", tokenResponse.RefreshToken).Result;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponseTwo.AccessToken);
            var responseTwo = await _httpClient.GetAsync($"/api/values");
            Assert.Equal(HttpStatusCode.OK, responseTwo.StatusCode);
        }

        [Fact]
        public async Task OAuth_ClientCredentials_Test()
        {
            var tokenResponse = GetToken("client_credentials").Result; //获取 access_token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

            var response = await _httpClient.GetAsync($"/api/values");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.StatusCode);
                //Console.WriteLine((await response.Content.ReadAsAsync<HttpError>()).Message);
            }
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Thread.Sleep(10000);

            var tokenResponseTwo = GetToken("refresh_token", tokenResponse.RefreshToken).Result;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponseTwo.AccessToken);
            var responseTwo = await _httpClient.GetAsync($"/api/values");
            Assert.Equal(HttpStatusCode.OK, responseTwo.StatusCode);
        }
    }
}