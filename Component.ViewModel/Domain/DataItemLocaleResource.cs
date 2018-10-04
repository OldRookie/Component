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
    [Table("Sys_DataItemLocaleResource")]
    [Description("数据本地化资源")]
    public class DataItemLocaleResource
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
        /// 资源名称
        /// </summary>
        [Display(Name = "资源值")]
        public string ResourceValue { get; set; }

        /// <summary>
        /// 数据项本地化资源类型
        /// </summary>
        [Display(Name = "资源类型")]
        public DataItemLocaleResourceType ResourceType { get; set; }
    }

    public enum DataItemLocaleResourceType {
        BaseData = 1
    }
}
