using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Component.Model.ViewModel
{
    public class ImportVM
    {
        [Display(Name = "附件")]
        public string File { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }
    }
}
