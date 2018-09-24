using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.Entity
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
