using Component.Infrastructure;
using Component.Infrastructure.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Component.UI.MVC.Framework
{
    public class AuthInfoHelper
    {
        public static void SetAuthInfo(string userName, string language = "ZH-CN")
        {
            var user = new IdentityInfo(); //new UserAppService().FindIdentityInfo(userName);
            user.UserName = userName;

            HttpContext.Current.Session.Clear();

            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.Formatting = Formatting.None;

            string UserData = JsonConvert.SerializeObject(user, setting).GZipCompressString();
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now,
                DateTime.Now.AddDays(1), false, UserData);
            HttpCookie Cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(Ticket));//加密身份信息，保存至Cookie
            HttpContext.Current.Response.Cookies.Add(Cookie);

            //FormsAuthentication.SetAuthCookie(userName, false);
        }
    }
}