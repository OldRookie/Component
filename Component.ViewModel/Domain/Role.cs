using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.Domain
{
    /// <summary>
    /// 权限组清单
    /// </summary>
    public class Role
    {
        [StringLength(256, ErrorMessage = "不能超过256个字符")]
        public string Id { get; set; }  //权限标识

        [StringLength(256, ErrorMessage = "不能超过256个字符")]
        public string Desc { get; set; }  //描述
    }
}
