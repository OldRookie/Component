using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Component.UI.MVC.Framework
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var langRouteKey = "lang";
            string _language = "";
            if (filterContext.RouteData.Values[langRouteKey] != null &&
                     !string.IsNullOrWhiteSpace(filterContext.RouteData.Values[langRouteKey].ToString()))
            {
                _language = filterContext.RouteData.Values[langRouteKey].ToString().ToUpper();
            }
            else
            {
                _language= CookieHelper.GetLanguageFromCookie(filterContext.HttpContext.Request.Cookies);
                if (_language.IsEmpty())
                {
                    _language = filterContext.HttpContext.Request.UserLanguages[0];
                } 
            }

            if (!_language.IsEmpty())
            {
                ///从路由数据(url)里设置语言                
                switch (_language)
                {
                    case "ZH-CN":
                    case "ZH-TW":
                    case "EN-US":
                        {
                            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(_language);
                            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(_language);
                            break;
                        }

                }
                filterContext.RouteData.Values[langRouteKey] = _language;

                HttpCookie _cookie = CookieHelper.UpdateLanguageToCookie(_language, filterContext.HttpContext.Request.Cookies);
                _cookie.Expires = DateTime.Now.AddYears(1);
                filterContext.HttpContext.Response.SetCookie(_cookie);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}