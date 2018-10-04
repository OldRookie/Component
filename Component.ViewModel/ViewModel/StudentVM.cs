using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Component.ViewModel
{
    public class StudentVM
    {
        public string Id { get; set; }

        [Display(Name ="名字")]
        public string Name { get; set; }

        [Display(Name = "班级")]
        public int ClassNumber { get; set; }

        [Display(Name = "年纪")]
        public int Grade { get; set; }
    }
}
