using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Component.ViewModel.WorkFlowViewMoels
{
    public class BaseFormVM
    {
        public string Id { get; set; }

        [Display(Name = "名字")]
        public string Name { get; set; }

        [Display(Name = "日期")]
        public DateTime? FormDateTime{ get; set; }

        [Display(Name = "金额")]
        public int? Money { get; set; }

        public List<FormDetailVM> FormDetail { get; set; }
    }
}
