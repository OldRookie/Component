using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Component.AuthorizationServer.Controllers
{
    public class AuthClientController : ApiController
    {
        [HttpGet]
        [Route("api/AuthorizationCode")]
        public HttpResponseMessage AuthorizationCode(string code)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(code, Encoding.UTF8, "text/plain")
            };
        }


        [HttpGet]
        [Route("api/ImplicitToken")]
        public HttpResponseMessage ImplicitToken()
        {
            var access_token = Request.RequestUri.OriginalString;
            return new HttpResponseMessage()
            {
                Content = new StringContent(access_token, Encoding.UTF8, "text/plain")
            };
        }
    }
}
