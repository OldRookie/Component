using Component.UI.Api.App_Start;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Component.UI.Api.Providers
{
    public class ApiAuthorizeAttribute: AuthorizationFilterAttribute
    {
        private static readonly string[] _emptyArray = new string[0];
        private readonly object _typeId = new object();
        private string _roles;
        private string[] _rolesSplit = ApiAuthorizeAttribute._emptyArray;
        private string _users;
        private string[] _usersSplit = ApiAuthorizeAttribute._emptyArray;


        /// <summary>Gets or sets the authorized roles. </summary>
        /// <returns>The roles string. </returns>
        public string Roles
        {
            get
            {
                return this._roles ?? string.Empty;
            }
            set
            {
                this._roles = value;
                this._rolesSplit = ApiAuthorizeAttribute.SplitString(value);
            }
        }

        /// <summary>Gets or sets the authorized users. </summary>
        /// <returns>The users string. </returns>
        public string Users
        {
            get
            {
                return this._users ?? string.Empty;
            }
            set
            {
                this._users = value;
                this._usersSplit = ApiAuthorizeAttribute.SplitString(value);
            }
        }


        /// <summary>Gets a unique identifier for this attribute.</summary>
        /// <returns>A unique identifier for this attribute.</returns>
        public override object TypeId
        {
            get
            {
                return this._typeId;
            }
        }


        /// <summary>
        /// The Token ParameterName in Get Url parameter or Payload in Post,Put etc.
        /// </summary>
        public string TokenParameterName { get; private set; }


        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parameterName">token parameter name</param>
        public ApiAuthorizeAttribute(string parameterName = "token")
        {
            this.TokenParameterName = parameterName.Trim();
        }


        /// <summary>
        ///  验证当前的
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected virtual bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }


            IPrincipal principal = actionContext.ControllerContext.RequestContext.Principal;
            bool isRegularAuth = principal != null && principal.Identity != null && principal.Identity.IsAuthenticated && (this._usersSplit.Length <= 0 || this._usersSplit.Contains(principal.Identity.Name, StringComparer.OrdinalIgnoreCase)) && (this._rolesSplit.Length <= 0 || this._rolesSplit.Any(new Func<string, bool>(principal.IsInRole)));

            if (isRegularAuth)
            {
                return true;
            }


            string token = null;
            HttpMethod method = actionContext.Request.Method;
            if (method.Equals(HttpMethod.Get) || method.Equals(HttpMethod.Delete))
            {
                token = actionContext.Request.GetQueryNameValuePairs()
                .FirstOrDefault(p => p.Key.Equals(this.TokenParameterName, StringComparison.CurrentCultureIgnoreCase)).Value;
            }
            else
            {
                if (actionContext.Request.Content.IsFormData())
                {
                    var postData = actionContext.Request.Content.ReadAsFormDataAsync().Result;
                    if (postData.AllKeys.Contains(this.TokenParameterName))
                    {
                        token = postData.Get(this.TokenParameterName);
                    }
                }
            }

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            AuthenticationTicket _AuthenticationTicket = StartupX509.JsonWebTokenFormatter.Unprotect(token.Trim());

            if (_AuthenticationTicket == null)
            {
                return false;
            }

            var identity = new StandardIdentity(_AuthenticationTicket.Identity.Name);
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = identity;
                return true;
            }

            return false;
        }


        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            if (ApiAuthorizeAttribute.SkipAuthorization(actionContext))
            {
                return;
            }

            if (!this.IsAuthorized(actionContext))
            {
                this.HandleUnauthorizedRequest(actionContext);
            }
        }

        protected void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            // Return Response
            Dictionary<string, string> responseData = new Dictionary<string, string>();
            responseData.Add("Message", "Authorization has been denied for this request.");
            HttpResponseMessage challengeMessage = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, responseData);
            challengeMessage.Headers.Add("WWW-Authenticate", "Basic");
            actionContext.Response = challengeMessage;
        }



        /// <summary>
        /// Skip Auth using  AllowAnonymousAttribute
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any<AllowAnonymousAttribute>() || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any<AllowAnonymousAttribute>();
        }


        /// <summary>
        /// Split String
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return ApiAuthorizeAttribute._emptyArray;
            }

            IEnumerable<string> source = from piece in original.Split(new char[] { ',' })
                                         let trimmed = piece.Trim()
                                         where !string.IsNullOrEmpty(trimmed)
                                         select trimmed;

            return source.ToArray<string>();
        }
    }

    public class StandardIdentity : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }

        public StandardIdentity(string name)
        {
            this.Identity = new GenericIdentity(name);
        }
    }

}