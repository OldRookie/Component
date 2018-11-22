using Component.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.Domain
{
    /// <summary>
    /// 用户组-权限清单
    /// </summary>
    public class RolePermission : BaseEntity
    {
        [StringLength(256, ErrorMessage = "不能超过256个字符")]
        public string UserGroupId { get; set; }  //权限组

        [ForeignKey("UserGroupId")]
        public virtual Role UserGroup { get; set; }

        [StringLength(256, ErrorMessage = "不能超过256个字符")]
        public string PermissionId { get; set; }    //权限ID

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}
