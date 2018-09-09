using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace Component.UI.MVC.Framework
{
    public class UserAuthorizeAttribute : BaseAuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!string.IsNullOrEmpty(this.Roles))
            {
                //if (!IdentityInfoHelper.CurrentIdentityInfo.Pemission.Contains(this.Roles))
                {
                    filterContext.Result = new RedirectResult("/");
                }
            }
        }
    }
}