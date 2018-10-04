using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.Domain
{
    [Table("WF_FormDetail")]
    public class FormDetail
    {
        public string Id { get; set; }

        [Display(Name = "标题")]
        public string Title { get; set; }

        [Display(Name = "描述")]
        public string Desc { get; set; }
    }
}
