using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Component.UI.MVC.Areas.Bootstrap.Controllers
{
    public class ImportController : Controller
    {
        // GET: Bootstrap/Import
        public ActionResult Upload()
        {
            return PartialView("_Upload");
        }
    }
}