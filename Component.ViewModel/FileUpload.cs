using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Component.ViewModel
{
    public class FileUpload
    {
        public FileUpload()
        {
            this.FileUploads = new List<HttpPostedFileBase>();
        }

        public List<HttpPostedFileBase> FileUploads { get; set; }
    }
}
