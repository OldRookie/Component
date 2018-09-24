using Component.Infrastructure.FileUpload;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Component.Application
{
    public class AttachmentApp
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="attamentTypeCode"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Hashtable UploadFile(HttpPostedFileBase file, Dictionary<string, string> parameter)
        {
            int maxSize = 52428800;
            Hashtable hash = new Hashtable();

            var uploadFileInfo = new UploadFileInfo();
            uploadFileInfo.FileStream = file.InputStream;
            uploadFileInfo.FileName = file.FileName;
            uploadFileInfo.Size = file.InputStream.Length;

            uploadFileInfo.DirPath = $"/UploadFileSpace/MgtFormAttachment";

            var fileUploadHelper = new PathFileUpload();
            fileUploadHelper.UploadFile(new List<UploadFileInfo> { uploadFileInfo });

            hash["error"] = 0;
            hash["url"] = uploadFileInfo.Url;
            hash["FileName"] = uploadFileInfo.FileName;
            hash["FileFullPath"] = uploadFileInfo.FileFullPath;
            hash["Size"] = uploadFileInfo.Size;

            return hash;
        }
    }
}