using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Component.UI.MVC.Controllers
{
    public class SettingController : Controller
    {
        // GET: Setting
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeLang(string language)
        {
            var referenceRouteData = new RouteValueDictionary();
            var controller = Request.QueryString["referencecontroller"].ToString();
            var action = Request.QueryString["referenceaction"].ToString();

            foreach (var key in Request.QueryString.AllKeys)
            {
                if (!key.Equals("lang", StringComparison.CurrentCultureIgnoreCase)
                    && !key.Equals("language", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!key.Contains("referencecontroller") && !key.Contains("referenceaction"))
                    {
                        referenceRouteData.Add(key, Request.QueryString[key]);
                    }
                }
            }
            referenceRouteData.Add("lang", language);
            return RedirectToAction(action, controller, referenceRouteData);
        }
    }
}