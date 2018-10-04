using Component.Infrastructure;
using Component.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Component.UI.MVC.Areas.Bootstrap.Controllers
{
    public class AttachmentController : Controller
    {
        // GET: Bootstrap/Attachment
        public ActionResult Upload()
        {
            var attchments = new List<AttachmentVM>();
            var result = new FileInputResult()
            {
                append = true,
                initialPreview = new List<string>(),
                initialPreviewConfig = new List<InitialPreviewConfig>()
            };

            foreach (string key in Request.Files.AllKeys)
            {
                var file = Request.Files[key];
                if (file != null && file.ContentLength > 0)
                {
                    var path = Server.MapPath("~/UploadImages/" + file.FileName);
                    attchments.Add(new AttachmentVM
                    {
                        Name = file.FileName,
                        Size = file.ContentLength,
                        Id = Guid.NewGuid()
                    });
                    file.SaveAs(path);
                }
            }

            foreach (var attchment in attchments)
            {
                result.initialPreview.Add("text");
                result.initialPreviewConfig.Add(new InitialPreviewConfig()
                {
                    caption = attchment.Name,
                    size = attchment.Size,
                    type = "text",
                    downloadUrl = Url.Action("Download", "Attachment", new { id = attchment.Id }),
                    key = attchment.Id.ToString()
                });
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }

      

        /// <summary>
        /// 下载
        /// </summary>
        /// <returns></returns>
        public FileResult Download(Guid Id) {
            AttachmentVM attachmentDTO = null;// _attachmentAppService.DownloadAttachment(Id);
            string filePath = Server.MapPath(attachmentDTO.Path);
            return File(filePath, "text/plain", attachmentDTO.Name);
        }

      
        public AttachmentVM DownloadAttachment(Guid Id) {
            var attachment = new object();
            //Attachment attachment = _attachmentRepository.Find(a => a.Id == Id).FirstOrDefault();
            AttachmentVM attachmentDTO = attachment.JsonAutoMapTo<AttachmentVM>();
            return attachmentDTO;
        }
    }

    
}