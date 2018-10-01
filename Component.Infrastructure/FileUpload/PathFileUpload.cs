using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Component.Infrastructure.FileUpload
{
    /// <summary>
    /// 文件上传帮助类
    /// </summary>
    public class PathFileUpload : IFileUpload
    {
        public byte[] GetFile(string filePath)
        {
            byte[] bytes = null;
            var path = FileHelper.MapPath(filePath);
            if (FileHelper.IsExistFile(path))
            {
                bytes = System.IO.File.ReadAllBytes(path);
            }
            return bytes;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="uploadFileInfos"></param>
        public void UploadFile(List<UploadFileInfo> uploadFileInfos)
        {
            if (uploadFileInfos.Count == 0)
            {
                return;
            }

            Dictionary<string, UploadFileInfo> webFilePathToFile = new Dictionary<string, UploadFileInfo>();

            foreach (var file in uploadFileInfos)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string fileFullPath = $"{file.DirPath}/{fileName}";
                file.FileFullPath = fileFullPath;
                file.Url = Configs.GetValue("FileServer") + file.FileFullPath;

                var environmentDir = HostingEnvironment.MapPath(file.DirPath);
                string webFilePath = Path.Combine(environmentDir, fileName);

                webFilePathToFile.Add(webFilePath, file);
            }

            foreach (var uploadFileInfo in uploadFileInfos)
            {
                var environmentDir = HostingEnvironment.MapPath(uploadFileInfo.DirPath);
                if (!FileHelper.IsExistDirectory(environmentDir))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        try
                        {
                            FileHelper.CreateDirectory(environmentDir);
                            break;
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(300);
                        }
                    }
                }
            }

            foreach (var item in webFilePathToFile)
            {
                using (var fileStream = System.IO.File.Create(item.Key))
                {
                    item.Value.FileStream.Seek(0, SeekOrigin.Begin);
                    item.Value.FileStream.CopyTo(fileStream);
                }
            }
        }
    }

    public interface IFileUpload
    {
        void UploadFile(List<UploadFileInfo> uploadFileInfos);

        byte[] GetFile(string filePath);
    }
}
