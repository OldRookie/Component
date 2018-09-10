using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Component.UI.Api.Providers
{
    public class OAuthProvider: OAuthAuthorizationServerProvider
    {
        private string logMsg;

        private readonly string _publicClientId;

        /// <summary>
        /// Default Construtor for Oauth Provider
        /// </summary>
        /// <param name="publicClientId"></param>
        public OAuthProvider(string publicClientId)
        {
            if (string.IsNullOrEmpty(publicClientId))
            {
                Exception ex = new ArgumentNullException("public clientId is null or empty in OAuth,Init System Fail");
                // To do the Log
                throw ex;
            }

            _publicClientId = publicClientId;
        }


        /// <summary>
        ///  Resource Owner Credentials
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            // Load from Cache
            // The map of the Current Useris registed in the Identity Server

            // Db Version
            // loginlUserMap = ExternalUserApp.Select(1, 1, 1, context.UserName);

            // Cache version

            //if (false)
            //{
            //    context.SetError("invalid_grant", "The user name is not registed to Identity Server");
            //    return Task.FromResult<object>(null);
            //}

            //if (externalClient != null && !string.IsNullOrEmpty(externalClient.UserValidationUrl))
            {
                // Start the validation. Try Cache in Application Error
                {
                    List<Claim> claimData = new List<Claim>()
                    {
                        //..
                        new Claim(ClaimTypes.Name,context.UserName)// Actor user name
                    };


                    ClaimsIdentity identity = new ClaimsIdentity(claimData, context.Options.AuthenticationType);
                    identity.Label = "Intertek Identity Server";
                    //identity.Actor = new ClaimsIdentity(new List<Claim> { new Claim("client_name", "") }, context.Options.AuthenticationType);

                    // Set property for JWT, It is Empty 
                    IDictionary<string, string> JWTAttributeData = new Dictionary<string, string>
                    {
                        //{ "IdentityServerVersion",Startup.AssemblyVersion.ToString()}
                    };

                    var props = new AuthenticationProperties(JWTAttributeData);
                    props.AllowRefresh = true;

                    var ticket = new AuthenticationTicket(identity, props);
                    context.Validated(ticket);
                    return base.GrantResourceOwnerCredentials(context);
                }// end response code
            } // end if url 


            context.SetError("invalid_grant", logMsg);
            return Task.FromResult<object>(null);
        }

        #region Credential
        /// <summary>
        /// Client Credentials
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            // Db Version
            // ExternalClientViewModel externalClientData = ExternalClientApp.FindBy(2, context.ClientId);

            //Cache Version


            // Claim Data
            List<Claim> claimData = new List<Claim>()
            {
            };

            ClaimsIdentity identity = new ClaimsIdentity(claimData, context.Options.AuthenticationType);
            identity.Label = "Intertek Identity Server";

            // Seet property for JWT
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                //{ "IdentityServerVersion",Startup.AssemblyVersion.ToString()}
            };
            var props = new AuthenticationProperties(data);
            props.AllowRefresh = true;

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

            return base.GrantClientCredentials(context);
        }


        /// <summary>
        /// Refresh Token Credentials
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            return base.GrantRefreshToken(context);
        }
        #endregion

        #region Validation

        /// <summary>
        /// Validation of the Appkey and App secret in the 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = null;
            string clientSecret = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (string.IsNullOrEmpty(clientId))
            {
                // Set Error 
                context.SetError("invalid_ClientId", "clientId is null");
                return Task.FromResult<object>(null);
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                // Set Error 
                context.SetError("invalid_Secrept", "clientSecret is null");
                return Task.FromResult<object>(null);
            }

            // Find the External Client

            //if (externalClient != null)
            {
                context.Validated(clientId);
                return base.ValidateClientAuthentication(context);
            }

            // Set Error
            context.SetError("invalid_client", "client_Id and client_Secret is invalid");
            return Task.FromResult<object>(null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }

                else if (context.ClientId == "Intertek.IdentityServer")
                {
                    var expectedUri = new Uri(context.Request.Uri, "/");
                    context.Validated(expectedUri.AbsoluteUri);
                }
            }

            return Task.FromResult<object>(null);
        }


        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }


        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            //Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            var items = context.AccessToken.Split('.').ToList();
            var userInfo = string.Empty;
            if (items.Count > 0)
            {
                int mm = items[1].Replace(" ", "").Length % 4;
                if (mm > 0)
                {
                    items[1] += new string('=', 4 - mm);
                }
                byte[] data = Convert.FromBase64String(items[1]);
                userInfo = Encoding.UTF8.GetString(data);
            }

            //var logMsg = string.Format("Token issued: [{0}] - Decoded token format [{1}]", context.AccessToken, userInfo);
            //LogModel = new SysApplicationLogViewModel("TokenEndpointResponse", context.Identity.Name, logMsg);
            //Logger.Debug(LogModel);
            return base.TokenEndpointResponse(context);
        }
        #endregion
    }
}