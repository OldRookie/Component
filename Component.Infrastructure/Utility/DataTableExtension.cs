using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure.Utility
{
    public static class DataTableExtension
    {
        public static DataTable ToDataTable<T>(this IList<T> data, Action<T, PropertyDescriptor> setColumn = null, Action<T, PropertyDescriptor> setValue = null)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                var name = prop.DisplayName;
                if (string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(prop.Description))
                {
                    name = prop.Description;
                }

                if (!string.IsNullOrEmpty(name))
                {
                    var column = new DataColumn(prop.DisplayName, prop.PropertyType);
                    if (prop.PropertyType.Name.Contains("DateTime"))
                    {
                        column = new DataColumn(prop.DisplayName, typeof(string));
                    }
                    table.Columns.Add(column);
                }
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                    if (setValue != null)
                    {
                        setValue(item, props[i]);
                    }
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }
}
