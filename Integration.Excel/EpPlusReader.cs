using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Integration.Excel.ExcelSettings;
using Integration.ToolKits;
using OfficeOpenXml;

namespace Integration.Excel
{
    public class EpPlusReader : IExcelReader, IDisposable
    {

        private ExcelPackage _excelPackage;
        private int _sheetIndex;
        public void Load(Stream stream)
        {
            _excelPackage = new ExcelPackage(stream);
            _sheetIndex = -1;
        }

        public void Load(byte[] bytes)
        {
            using var memory = new MemoryStream(bytes);
            Load(memory);
        }

        public IEnumerable<T> Read<T>(ReadSheetSettings<T> settings) where T : class, IReadData, new()
        {
            if (_excelPackage == null)
            {
                throw new Exception("读取Excel失败, 请先Load()。");
            }

            _sheetIndex++;
            var startRow = 1;

            var startCol = 1;
            if (settings.HasOrdinalColumn)
            {
                startCol = 2;
            }

            //
            var sheet = string.IsNullOrEmpty(settings.SheetName)
                ? _excelPackage.Workbook.Worksheets[_sheetIndex]
                : _excelPackage.Workbook.Worksheets[settings.SheetName];

            if (sheet.Dimension?.End == null)
            {
                return Enumerable.Empty<T>();
            }

            var titles = ReadTitle(sheet, startRow, startCol);
            if (titles.Count == 0)
            {
                throw new BizException($"在工作表{sheet.Name}中没有读取到标题");
            }

            var repeats = titles
                .GroupBy(it => it.Title)
                .Where(it => it.Count() > 1)
                .Select(it => it.Key)
                .Distinct()
                .ToArray();

            if (repeats.Length > 0)
            {
                throw new BizException($"在工作表{sheet.Name}中存在重复标题:{string.Join(',', repeats)}");
            }


            foreach (var c in settings.Columns)
            {
                var kv = titles.FirstOrDefault(t => t.Title == c.Title);
                if (kv != null)
                {
                    c.Index = kv.Index;
                }
                else
                {
                    throw new BizException($"在工作表{sheet.Name}中没有发现标题:{c.Title}");
                }
            }
            startRow++;

            var maxRow = sheet.Dimension.End.Row;
            var list = new List<T>();
            for (var i = startRow; i <= maxRow; i++)
            {
                var r = ReadRow(sheet, i, settings.Columns);
                list.Add(r);
            }
            return list;
        }

        private List<TitleIndex> ReadTitle(ExcelWorksheet sheet, int startCol, int startRow)
        {
            //var maxRow = sheet.Dimension.End.Row;
            var maxCol = sheet.Dimension.End.Column;
            var list = new List<TitleIndex>();
            for (var i = startCol; i <= maxCol; i++)
            {
                var v = sheet.Cells[startRow, i].Value;
                if (v != null)
                {
                    list.Add(new TitleIndex { Title = v.ToString(), Index = i });
                }
            }
            return list;
        }

        private T ReadRow<T>(ExcelWorksheet sheet, int row, List<IReadColumnSetting<T>> columns) where T : class, IReadData, new()
        {
            var data = new T
            {
                RowNumber = row,
                ErrorMessages = new List<string>()

            };
            foreach (var column in columns)
            {
                var v = sheet.Cells[row, column.Index].Value ?? column.DefaultValue;
                if (v == null && column.IsRequired)
                {
                    data.ErrorMessages.Add($"{column.Title}必填");
                    continue;
                }
                column.Set(v, data);
            }

            return data;
        }

        public void Dispose()
        {
            _excelPackage?.Dispose();
        }

        /// <summary>
        /// 获取sheet数量
        /// </summary>
        /// <returns></returns>
        public int GetSheetCount()
        {
            return _excelPackage.Workbook.Worksheets.Count;
        }
    }
}
