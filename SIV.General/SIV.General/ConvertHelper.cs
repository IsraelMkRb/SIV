using System;
using System.Data;
using System.Reflection;

namespace SIV.General
{
    public static class ConvertHelper
    {
        public static T ConvertDataReaderToObject<T>(IDataReader reader) where T : new()
        {
            T obj = new T();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                PropertyInfo property = FindProperty(typeof(T), columnName);

                if (property != null && reader[i] != DBNull.Value)
                {
                    object value = reader[i];
                    property.SetValue(obj, value);
                }
            }

            return obj;
        }

        private static PropertyInfo FindProperty(Type type, string columnName)
        {
            PropertyInfo property = type.GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null && type.BaseType != null)
            {
                property = FindProperty(type.BaseType, columnName);
            }

            return property;
        }
    }
}
