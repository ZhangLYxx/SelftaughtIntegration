using System;
using System.Reflection;

namespace Integration.Excel.ExcelSettings
{
    /// <summary>
    /// 列设置
    /// </summary>
    public interface IColumnSetting
    {
        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; }


        /// <summary>
        /// 数据类型
        /// </summary>
        Type DataType { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        PropertyInfo PropertyInfo { get; set; }
    }

    public record sclass{
        public int MyProperty { get; set; }
    }
}
