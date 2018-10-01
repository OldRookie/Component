using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure.FileUpload
{
    /// <summary>
    /// 上传文件信息
    /// </summary>
    public class UploadFileInfo
    {
        public Stream FileStream { get; set; }

        public string FileName { get; set; }

        public string DirPath { get; set; }

        public string FileFullPath { get; set; }

        public long Size { get; set; }

        public string Url { get; set; }
    }

    public static class UploadFileInfoExtension
    {

        public static string FilePath(string relativePath)
        {
            return Configs.GetValue("ImageServer") + relativePath;
        }
    }
}
