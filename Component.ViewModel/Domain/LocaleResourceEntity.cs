using Component.Infrastructure.BaseDataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.Domain
{
    /// <summary>
    /// 本地环境资源
    /// </summary>
    [Table("Sys_LocaleResource")]
    [Description("本地化资源")]
    public class LocaleResource
    {
        /// <summary>
        /// 主键
        /// </summary>
        [StringLength(50)]
        [Display(Name = "主键")]
        public string Id { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        [Display(Name = "语言")]
        [StringLength(50)]
        public string Language { get; set; }


        /// <summary>
        /// 资源模块,{ModuleName}.{ModuleName}
        /// </summary>
        [Display(Name = "资源模块,{ModuleName}.{ModuleName}")]
        [StringLength(1024)]
        public string ResourceModule { get; set; }

        /// <summary>
        /// 资源代码
        /// </summary>
        [Display(Name = "资源代码")]
        [StringLength(1024)]
        public string ResourceCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        [Display(Name = "资源值")]
        public string ResourceValue { get; set; }

        /// <summary>
        /// 本地环境区域资源类型
        /// </summary>
        [Display(Name = "资源类型")]
        public LocaleResourceTypeCode LocaleResourceType { get; set; }
    }

    public enum LocaleResourceTypeCode {

    }
}
