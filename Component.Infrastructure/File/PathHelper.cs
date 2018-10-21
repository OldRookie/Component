using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Component.Infrastructure
{
    public class PathHelper
    {
        /// <summary>
        /// 是否为有效图片文件
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <returns></returns>
        public static bool IsValidImageFormat(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }
            var extension = Path.GetExtension(path);
            return new string[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(extension.ToLower());
        }

        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public static string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }

            //not hosted. For example, run in unit tests
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }
    }

}
