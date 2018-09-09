using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Component.UI.MVC.Framework
{
    public class BaseAuthorizeAttribute : AuthorizeAttribute
    {
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
    }
}