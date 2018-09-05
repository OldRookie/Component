using Component.Model.DataTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.ViewModel
{
    public class DataTablesQueryRequest: DataTablesParameters
    {
        [Display(Name="名字")]
        public string Name { get; set; }
    }
}
