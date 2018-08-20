using Component.ViewModel.WorkFlowViewMoels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Component.UI.MVC.Areas.Bootstrap.Controllers
{
    public class FormsController : Controller
    {
        // GET: Bootstrap/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BaseForm()
        {
            var model = new BaseFormVM();
            model.FormDetail = new List<FormDetail>()
            {
                new FormDetail(),new FormDetail()
            };
            return View(model);
        }

        public ActionResult SaveBaseForm(BaseFormVM vm)
        {
            return View();
        }
    }
}