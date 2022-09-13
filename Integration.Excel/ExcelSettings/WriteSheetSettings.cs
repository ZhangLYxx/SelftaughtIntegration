using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;


namespace Integration.Excel.ExcelSettings
{

    public interface ISheetSettings
    {
        /// <summary>
        /// 工作表名称
        /// </summary>
        string SheetName { get; set; }

        bool HasOrdinalColumn { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        int ColumnCount { get; set; }

    }


    /// <summary>
    /// Excel Sheet 设置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WriteSheetSettings<T> : ISheetSettings where T : class
    {

        public WriteSheetSettings()
        {
            ColumnSettings = new List<IWriteColumnSetting>();
        }

        public int ColumnCount { get; set; }

        /// <summary>
        /// 工作表名称
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public IEnumerable<T> DataSources { get; set; }

        /// <summary>
        /// 字段设置
        /// </summary>
        public List<IWriteColumnSetting> ColumnSettings { get; set; }


        /// <summary>
        /// 是否有 序号 列
        /// </summary>
        public bool HasOrdinalColumn { get; set; }


        /// <summary>
        /// 属性配置
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public WriteColumnSetting<T> Property<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess && propertyExpression.Body is MemberExpression exp)
            {
                if (exp.Member.MemberType != MemberTypes.Property)
                {
                    throw new Exception($"SheetSettings.Property()方法 只支持 属性，{exp.Member.Name} is {exp.Member.MemberType}");
                }
                var c = new WriteColumnSetting<T>
                {
                    ColumnName = exp.Member.Name,
                    PropertyInfo = exp.Member as PropertyInfo,
                    DataType = exp.Member.DeclaringType
                };
                ColumnSettings.Add(c);
                c.Index = ColumnSettings.IndexOf(c);

                return c;
            }
            throw new Exception("无效的表达式");
        }

        public WriteColumnSetting<T> HasFuncColumn(Expression<Func<T, object>> funcExpression)
        {
            var func = funcExpression.Compile();
            var c = new WriteColumnSetting<T>
            {
                ConvertFunc = func
            };
            ColumnSettings.Add(c);
            return c;
        }

        /// <summary>
        /// 列分组
        /// </summary>
        /// <typeparam name="TGroup"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public WriteColumnGroup<T, TGroup> HasGroupColumn<TGroup>(Expression<Func<T, IEnumerable<TGroup>>> propertyExpression) where TGroup : class
        {
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess && propertyExpression.Body is MemberExpression exp)
            {
                if (exp.Member.MemberType != MemberTypes.Property)
                {
                    throw new Exception($"SheetSettings.HasSplitColumn()方法 只支持 属性，{exp.Member.Name} is {exp.Member.MemberType}");
                }

                var p = exp.Member as PropertyInfo;
                var c = new WriteColumnGroup<T, TGroup>(this)
                {
                    Title = exp.Member.Name,
                    PropertyInfo = p,
                    DataType = p.PropertyType,
                    GetFunction = propertyExpression.Compile(),
                };
                ColumnSettings.Add(c);
                c.Index = ColumnSettings.IndexOf(c);
                return c;
            }
            throw new Exception("无效的表达式");
        }

    }
}
