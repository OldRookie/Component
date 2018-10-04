using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.ViewModel.WorkFlowViewMoels
{
    public class AdvancedForm
    {
        [Display(Name = "国家")]
        public string Country { get; set; }

        [Display(Name = "描述")]
        public string Desctription { get; set; }

        [Display(Name = "附件")]
        public Guid Attachmentid { get; set; }
    }
}
