using System;

namespace Integration.Excel.ExcelSettings
{
    public interface IReadColumnSetting : IColumnSetting
    {
        int Index { get; set; }

        bool IsRequired { get; set; }

        object DefaultValue { get; set; }
    }


    public interface IReadColumnSetting<T> : IReadColumnSetting where T : class, IReadData, new()
    {
        Action<object, T> Set { get; set; }


    }


}
