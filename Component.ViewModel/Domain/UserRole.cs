using Component.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.Domain
{
    public class UserRole : BaseEntity
    {
        public Guid UserID { get; set; }  //账号

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
