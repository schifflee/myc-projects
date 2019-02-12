using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.ComponentModel;

namespace MyKTV_Management_Studio
{
    public static class DataTableOperation
    {
        public static void LocalizeColumnHeaderText(DataTable dt,string[] words)
        {
            if (dt.Columns.Count==words.Length)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].Caption = words[i];
                }
            }            
        }

        public static DataTable ConvertToDataTable<T>(this IList<T> array)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dt.Columns.Add(property.Name, property.PropertyType);
            }
            object[] values = new object[properties.Count];
            foreach (T item in array)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
    }
}
