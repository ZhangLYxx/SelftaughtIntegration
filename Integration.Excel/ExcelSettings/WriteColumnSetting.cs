using System;
using System.Collections.Generic;
using System.Reflection;

namespace Integration.Excel.ExcelSettings
{
    public class WriteColumnSetting<T> : IWriteColumnSetting<T> where T : class
    {

        public WriteColumnSetting()
        {

        }


        public string ColumnName { get; set; }

        public string Title { get; private set; }

        public int Index { get; set; }

        public string Format { get; set; }

        public Type DataType { get; set; }

        public Func<T, object> ConvertFunc { get; set; }

        public PropertyInfo PropertyInfo { get; set; }


        public bool IsGroup { get; protected set; }

        public virtual object GetValue(object data)
        {
            if (ConvertFunc != null)
            {
                return ConvertFunc(data as T);
            }
            return PropertyInfo.GetValue(data);
        }

        public virtual int GetRowCount(T r)
        {
            return 1;
        }


        public WriteColumnSetting<T> HasTitle(string title)
        {
            Title = title;
            return this;
        }

        public WriteColumnSetting<T> HasFormat(string format)
        {
            Format = format;
            return this;
        }

        public WriteColumnSetting<T> HasConvert(Func<T, object> func)
        {
            ConvertFunc = func;
            return this;
        }

        public virtual IEnumerable<IWriteColumnSetting> GetColumnSettings()
        {
            return new[] { this };
        }

    }


}
