using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Integration.Excel.ExcelSettings
{
    public class ReadSheetSettings<T> : SheetSettingsBase<T> where T : class, IReadData, new()
    {
        public ReadSheetSettings()
        {
            Columns = new List<IReadColumnSetting<T>>();
        }
        public List<IReadColumnSetting<T>> Columns { get; set; }


        public ReadColumnSetting<T> Property<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess && propertyExpression.Body is MemberExpression exp)
            {
                if (exp.Member.MemberType != MemberTypes.Property)
                {
                    throw new Exception($"Property()方法 只支持 属性，{exp.Member.Name} is {exp.Member.MemberType}");
                }

                var p = exp.Member as PropertyInfo;
                if (p == null || !p.CanWrite)
                {
                    throw new Exception($"Property()方法 不支持只读属性 {exp.Member.Name} ");
                }

                var c = new ReadColumnSetting<T>
                {
                    PropertyInfo = p,
                    DataType = p.PropertyType
                };
                Columns.Add(c);
                return c;
            }
            throw new Exception("无效的表达式");
        }

    }


}
