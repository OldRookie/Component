using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.ViewModel.DTO
{
    public class WorkflowBaseInfoVM
    {
        public string Id { get; set; }

        [Display(Name = "序列号")]
        public string SequenceNumber { get; set; }

        [Display(Name = "类型")]
        public string Type { get; set; }

        [Display(Name = "创建日期")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "用户")]
        public string UserName { get; set; }
    }
}
