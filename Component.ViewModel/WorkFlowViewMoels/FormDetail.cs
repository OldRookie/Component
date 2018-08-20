using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Component.ViewModel.WorkFlowViewMoels
{
    public class FormDetail
    {
        public string Id { get; set; }

        [Display(Name = "标题")]
        public string Title { get; set; }

        [Display(Name = "描述")]
        public string Desc { get; set; }
    }
}
