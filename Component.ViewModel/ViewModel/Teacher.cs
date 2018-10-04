using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Component.ViewModel
{
    public class Teacher: BaseViewModel
    {
        [Display(Name = "名字")]
        public string Name { get; set; }
    }
}
