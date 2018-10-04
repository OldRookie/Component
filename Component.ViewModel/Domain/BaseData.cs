using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.Domain
{
    [Table("Sys_BaseData")]
    public class BaseData
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public string BaseDataTypeId { get; set; }
    }
}
