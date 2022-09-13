using System.Collections.Generic;

namespace Integration.Excel.ExcelSettings
{
    public interface IWriteColumnSetting : IColumnSetting
    {
        int Index { get; set; }

        string Format { get; set; }


        bool IsGroup { get; }

        IEnumerable<IWriteColumnSetting> GetColumnSettings();

        object GetValue(object r);


    }

    public interface IWriteColumnSetting<in T> : IWriteColumnSetting where T : class
    {
        int GetRowCount(T r);
    }
}
