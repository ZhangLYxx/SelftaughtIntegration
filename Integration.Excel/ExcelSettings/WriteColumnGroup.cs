using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Integration.Excel.ExcelSettings
{
    public class WriteColumnGroup<T, TData> : IWriteColumnSetting, IWriteColumnGroup
        where TData : class
        where T : class
    {


        public WriteColumnGroup(ISheetSettings sheet)
        {
            Sheet = sheet;
            IsGroup = true;
            Index = sheet.ColumnCount;
            Columns = new List<IWriteColumnSetting>();
        }

        public bool IsGroup { get; }

        public int Index { get; set; }

        public ISheetSettings Sheet { get; }

        public List<IWriteColumnSetting> Columns { get; set; }

        public Func<T, IEnumerable<TData>> GetFunction { get; set; }

        public string Format { get; set; }

        public string Title { get; set; }

        public Type DataType { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// 属性配置
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public WriteColumnSetting<TData> Property<TProperty>(Expression<Func<TData, TProperty>> propertyExpression)
        {
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess && propertyExpression.Body is MemberExpression exp)
            {
                if (exp.Member.MemberType != MemberTypes.Property)
                {
                    throw new Exception($"ExcelColumnGroup.Property()方法 只支持 属性，{exp.Member.Name} is {exp.Member.MemberType}");
                }

                var p = exp.Member as PropertyInfo;
                var c = new WriteColumnSetting<TData>
                {
                    ColumnName = exp.Member.Name,
                    PropertyInfo = p,
                    DataType = p.PropertyType,
                };
                Columns.Add(c);
                c.Index = Columns.IndexOf(c);
                return c;
            }
            throw new Exception("无效的表达式");
        }

        public IEnumerable<IWriteColumnSetting> GetColumnSettings()
        {
            return Columns;
        }

        public int GetRowCount(T data)
        {
            var values = GetValues(data);
            if (values != null)
            {
                return values.Count();
            }
            return 1;
        }


        public IEnumerable<object> GetValues<TInput>(TInput data) where TInput : class
        {
            return GetFunction.Invoke(data as T);
        }


        public int GetRowCount<TInput>(TInput data) where TInput : class
        {
            var values = GetValues(data as T);
            if (values == null)
            {
                return 0;
            }
            return values.Count();
        }

        public object GetValue(object r)
        {
            throw new NotImplementedException();
        }
    }
}
