using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Security;

namespace Component.UI.MVC.Framework
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!string.IsNullOrEmpty(this.Roles))
            {
                //if (!IdentityInfoHelper.CurrentIdentityInfo.Pemission.Contains(this.Roles))
                {
                    filterContext.Result = new RedirectResult("/Auth/Login");
                }
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            IPrincipal user = httpContext.User;
            //|| HttpContext.Current.Session["UserInfo"] == null
            if (!user.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                return false;
            }
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
            if (filterContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            IPrincipal user = filterContext.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult($"/Auth/Login?returnUrl={filterContext.HttpContext.Request.RawUrl}");
            }
        }
    }
}