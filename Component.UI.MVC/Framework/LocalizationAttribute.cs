using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Component.UI.MVC.Framework
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //lang:
            //zh-CN
            //en-US
            string _language = "";
            if (filterContext.RouteData.Values["Lang"] != null &&
                     !string.IsNullOrWhiteSpace(filterContext.RouteData.Values["Lang"].ToString()))
            {
                ///从路由数据(url)里设置语言                
                switch (filterContext.RouteData.Values["Lang"].ToString().ToUpper())
                {
                    case "ZH-CN":
                    case "ZH-TW":
                    case "EN-US":
                        {
                            _language = filterContext.RouteData.Values["Lang"].ToString();
                            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(_language);
                            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(_language);
                            break;
                        }

                }
            }
            else
            {

            }

            base.OnActionExecuting(filterContext);
        }
    }
}