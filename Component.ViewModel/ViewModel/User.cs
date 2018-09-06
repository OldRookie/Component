﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.ViewModel
{
    public class UserVM
    {
        public string Id { get; set; }

        [Display(Name = "名字")]
        public string Name { get; set; }

        [Display(Name = "邮箱")]
        public string EMail { get; set; }

        [Display(Name="全名")]
        public string FullName { get; set; }
    }
}