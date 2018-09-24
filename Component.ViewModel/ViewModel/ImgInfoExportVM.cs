using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.ViewModel
{
    public class ImgInfoExportVM : ImgInfoVM
    {
        [Display(Name = "是否选中")]
        public string IsSelected { get; set; }


        [Display(Name = "数据源")]
        public string DataSource { get; set; }
    }
}
