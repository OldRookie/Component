using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.ModelMetaData
{
    public class UserMetadata
    {
        public string Id { get; set; }

        [Display(Name = "名字")]
        public string Name { get; set; }

        [Display(Name = "邮箱")]
        [Required]
        [EmailAddress]
        public string EMail { get; set; }

        [Display(Name = "全名")]
        [Required]
        public string FullName { get; set; }
    }
}
