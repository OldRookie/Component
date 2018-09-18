using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.ViewModel
{
    public class ImgInfoVM
    {
        [Display(Name = "申请单号")]
        public string SerialNumber { get; set; }

        [MaxLength(256)]
        [Display(Name = "编号")]
        public string FileName { get; set; }

        [MaxLength(1024)]
        public string FileFullPath { get; set; }

        [MaxLength(256)]
        [Display(Name = "用户")]
        public string UserName { get; set; }

        public string Url { get; set; }

        [Display(Name = "时间")]
        public DateTime? CreatorTime { get; set; }
    }
}
