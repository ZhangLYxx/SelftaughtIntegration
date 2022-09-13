using System;
using System.Collections.Generic;
using System.IO;

namespace Integration.Excel.ExcelSettings
{
    public interface IExcelReader
    {
        void Load(Stream stream);

        void Load(byte[] bytes);

        //void Load(ReadOnlySpan<byte> bytes);

        IEnumerable<T> Read<T>(ReadSheetSettings<T> settings) where T : class, IReadData, new();

        int GetSheetCount();
    }
}
