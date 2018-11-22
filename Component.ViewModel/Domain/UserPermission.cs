using Component.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.Domain
{
    /// <summary>
    /// 员工权限
    /// </summary>
    public class UserPermission : BaseEntity
    {
        public Guid UserID { get; set; }  //账号

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public string PermissionId { get; set; }  //
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}
