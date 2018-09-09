using Component.Model.DataTables;
using Component.UI.MVC.Framework;
using Component.ViewModel;
using Component.ViewModel.DTO;
using Component.ViewModel.WorkFlowViewMoels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Component.UI.MVC.Areas.Bootstrap.Controllers
{
    public class FormsController : BaseController
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

        public ActionResult AdvanceForm()
        {
            var model = new AdvancedForm();
            return View(model);
        }

        public ActionResult SaveBaseForm(BaseFormVM vm)
        {
            return View();
        }

        public ActionResult Query(DataTablesQueryRequest queryRequest)
        {
            var workFlowDTOs = new List<WorkFlowDTO>();
            workFlowDTOs.Add(new WorkFlowDTO()
            {
                Id = "1",
                CreateTime = DateTime.Now,
                SequenceNumber = "1",
                Type = "Type",
                UserName = "User"

            });
            var dataResult= new DataTablesResult<WorkFlowDTO>(queryRequest.Draw, workFlowDTOs.Count, workFlowDTOs.Count, workFlowDTOs);
            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DataTables()
        {
            return View();
        }
    }
}