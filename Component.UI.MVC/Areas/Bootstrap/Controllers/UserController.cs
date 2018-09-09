using Component.Infrastructure;
using Component.Model;
using Component.Model.DataTables;
using Component.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Component.UI.MVC.Areas.Bootstrap.Controllers
{
    public class UserController : Controller
    {
        // GET: Bootstrap/User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateOrEdit()
        {
            return PartialView("_CreateOrEdit");
        }

        public ActionResult Save(UserVM UserVM)
        {
            var responseResult = new ResponseResultBase();
            responseResult.Code = ResultCode.Success;
            var jsonResult = new JsonResult()
            {
                Data = responseResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return jsonResult;
        }

        public ActionResult Import(ImportVM importVM)
        {
            var responseResult = new ResponseResultBase();
            responseResult.Code = ResultCode.Failtrue;
            responseResult.ErrorMessages = new List<string>() { "Fail" };
            var jsonResult = new JsonResult()
            {
                Data = responseResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return jsonResult;
        }

        public ActionResult List()
        {
            var responseResult = new ResponseResultBase();
            return View();
        }

        public ActionResult Query(UserQueryRequest queryRequest)
        {
            var data = new List<UserVM>();
            data.Add(new UserVM()
            {
                Id = "1",
                FullName = "FullName",
                EMail = "122@12.com",
                Name = "Name"

            });
            data.Add(new UserVM()
            {
                Id = "1",
                FullName = "FullName",
                EMail = "122@12.com",
                Name = "Name"

            });
            var dataResult = new DataTablesResult<UserVM>(queryRequest.Draw, data.Count, data.Count, data);
            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }
    }
}