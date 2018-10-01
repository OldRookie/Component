using Component.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.Entity
{
    [Description("权限")]
    public class Permission
    {
        [Key]
        [StringLength(50)]
        [Display(Name = "主键")]
        public string Id { get; set; }

        [Display(Name = "模块")]
        public ModuleCode? Module { get; set; }

        [Display(Name = "描述")]
        public string Desc { get; set; }

        public virtual List<PermissionItem> PermissionItem { get; set; }
    }

    public enum ModuleCode
    {
        DrawingManage = 1
    }


    [Description("权限项")]
    public class PermissionItem
    {
        [Key]
        [StringLength(50)]
        [Display(Name = "主键")]
        public string Id { get; set; }

        [StringLength(50)]
        [Display(Name = "权限项")]
        public string ItemId { get; set; }

        [Display(Name = "权限项类型")]
        public PermissionItemTypeCode PermissionItemType { get; set; }

        [ForeignKey("Permission")]
        [Display(Name = "权限Id")]
        public string PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
    }

    public enum PermissionItemTypeCode
    {
        Menu = 1,
        Button = 2
    }
}
