using System.Collections.Generic;


namespace Integration.Excel.ExcelSettings
{
    public interface IWriteColumnGroup : IWriteColumnSetting
    {

        IEnumerable<object> GetValues<TInput>(TInput obj) where TInput : class;


        int GetRowCount<TInput>(TInput r) where TInput : class;
    }
}
