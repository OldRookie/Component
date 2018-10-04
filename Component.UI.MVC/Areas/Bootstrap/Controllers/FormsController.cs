using Component.Application.Read;
using Component.Model.DataTables;
using Component.Model.ViewModel;
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
using Webdiyer.WebControls.Mvc;

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
            model.FormDetail = new List<FormDetailVM>()
            {
                new FormDetailVM(),new FormDetailVM()
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
            return View("BaseForm");
        }

        public ActionResult Query(DataTablesQueryRequest queryRequest)
        {
            var workFlowDTOs = new List<WorkflowBaseInfoVM>();
            workFlowDTOs.Add(new WorkflowBaseInfoVM()
            {
                Id = "1",
                CreateTime = DateTime.Now,
                SequenceNumber = "1",
                Type = "Type",
                UserName = "User"

            });
            var dataResult= new DataTablesResult<WorkflowBaseInfoVM>(queryRequest.Draw, workFlowDTOs.Count, workFlowDTOs.Count, workFlowDTOs);
            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DataTables()
        {
            return View();
        }

        public ActionResult Export()
        {
            var fileName = "数据列表.xlsx";
            var items = new List<ImgInfoVM>();
            for (int i = 0; i < 20; i++)
            {
                items.Add(new ImgInfoVM()
                {
                    UserName = "UserName" + i.ToString(),
                    CreatorTime = DateTime.Now,
                    FileFullPath = "~/UploadImages/20080405144414541.png",
                    FileName = "FileName" + i.ToString(),
                    SerialNumber = "SerialNumber" + i.ToString(),
                    Url = ("/UploadImages/20080405144414541.png")
                });
            }

            var fileStream = new ImgInfoReadApp().Export(items);
            //fileStream.Position = 0;
            return File(fileStream, "application/ms-excel", fileName);
        }


        public ActionResult ExportComplexData()
        {
            var fileName = "复杂数据.xlsx";
            var items = new List<ImgInfoExportVM>();
            for (int i = 0; i < 20; i++)
            {
                items.Add(new ImgInfoExportVM()
                {
                    UserName = "UserName" + i.ToString(),
                    CreatorTime = DateTime.Now,
                    FileFullPath = "~/UploadImages/20080405144414541.png",
                    FileName = "FileName" + i.ToString(),
                    SerialNumber = "SerialNumber" + i.ToString(),
                    Url = ("/UploadImages/20080405144414541.png")
                });
            }

            var fileStream = new ImgInfoReadApp().ExportComplexData(items);
            //fileStream.Position = 0;
            return File(fileStream, "application/ms-excel", fileName);
        }
        public ActionResult QueryImgItem(string name, int pageNum = 1)
        {
            var items = new List<ImgInfoVM>();
            for (int i = 0; i < 20; i++)
            {
                items.Add(new ImgInfoVM()
                {
                    UserName = "UserName" + i.ToString(),
                    CreatorTime = DateTime.Now,
                    FileFullPath = "~/UploadImages/20080405144414541.png",
                    FileName = "FileName" + i.ToString(),
                    SerialNumber = "SerialNumber" + i.ToString(),
                    Url = ("/UploadImages/20080405144414541.png")
                });
            }
            var pageSourceData = items
                           .OrderByDescending(x => x.CreatorTime)
                           .ToPagedList(pageNum, 12);


            var result = new PagedList<ImgInfoVM>(pageSourceData,
                pageSourceData.CurrentPageIndex, pageSourceData.PageSize, pageSourceData.TotalItemCount);

            return PartialView("_PageImgItems", result);
        }

        public ActionResult CreateOrUpdateFormDetailItem()
        {
            return PartialView("_FormDetail", new FormDetailVM());
        }

        public ActionResult ImgItemList()
        {
            return View();
        }
    }
}