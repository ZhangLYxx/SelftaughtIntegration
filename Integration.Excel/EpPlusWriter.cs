using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Integration.Excel.ExcelSettings;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Integration.Excel
{
    public class EpPlusWriter : IExcelWriter
    {
        private ExcelPackage _excelPackage;


        public byte[] Save()
        {
            try
            {
                return _excelPackage.GetAsByteArray();
            }
            finally
            {
                if (_excelPackage != null)
                {
                    _excelPackage.Dispose();
                }
            }
        }

        public void Write<T>(WriteSheetSettings<T> sheetSettings) where T : class
        {
            CreateExcelIfNull();

            var sheet = _excelPackage.Workbook.Worksheets.Add(sheetSettings.SheetName ?? $"sheet{_excelPackage.Workbook.Worksheets.Count + 1}");
            //var data = sheetSettings.DataSources.ToArray();

            //var columns = sheetSettings.ColumnSettings.OrderBy(it => it.Index).ToArray();


            var startIndex = 1;

            var currentRow = WriteHeader(sheet, startIndex, sheetSettings.ColumnSettings, sheetSettings.HasOrdinalColumn, out var colEnd);

            currentRow++;
            WriteBody(sheet, currentRow, sheetSettings.ColumnSettings, sheetSettings.DataSources, sheetSettings.HasOrdinalColumn, colEnd);

            //不使用自动调整列宽，docker 环境下需要 libgdiplus
            //sheet.Cells.AutoFitColumns(8.43);
        }

        /// <summary>
        /// 头信息
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="startRow"></param>
        /// <param name="columnSettings"></param>
        /// <param name="hasOrdinalColumn"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        private int WriteHeader(ExcelWorksheet sheet, int startRow, List<IWriteColumnSetting> columnSettings, bool hasOrdinalColumn, out int colIndex)
        {
            var currentRow = startRow;
            colIndex = 1;
            if (hasOrdinalColumn)
            {
                sheet.SetValue(currentRow, colIndex, "序号");
                colIndex++;
            }
            foreach (var c in columnSettings)
            {
                if (!c.IsGroup)
                {
                    sheet.SetValue(currentRow, colIndex, c.Title);
                    if (c.DataType == typeof(DateTime))
                    {
                        sheet.Column(colIndex).Width = 19;
                    }
                    c.Index = colIndex++;
                }
                else
                {
                    foreach (var col in c.GetColumnSettings())
                    {
                        sheet.SetValue(currentRow, colIndex, col.Title);
                        if (c.DataType == typeof(DateTime))
                        {
                            sheet.Column(colIndex).Width = 19;
                        }
                        col.Index = colIndex++;
                    }
                }
            }
            var area = sheet.Cells[startRow, 1, currentRow, colIndex - 1];
            area.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            area.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            area.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            area.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            area.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            area.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            area.Style.WrapText = false;
            return currentRow;
        }


        /// <summary>
        /// 写数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet"></param>
        /// <param name="startRow"></param>
        /// <param name="columnSettings"></param>
        /// <param name="dataSources"></param>
        /// <param name="hasOrdinalColumn"></param>
        /// <param name="columnEnd"></param>
        /// <returns></returns>
        private int WriteBody<T>(ExcelWorksheet sheet, int startRow, List<IWriteColumnSetting> columnSettings,
            IEnumerable<T> dataSources,
            bool hasOrdinalColumn,
            int columnEnd)
        where T : class
        {
            var data = dataSources.ToArray();
            if (data.Length == 0)
            {
                return startRow;
            }
            var currentRow = startRow;
            //var columnStart = 1;
            //if (hasOrdinalColumn)
            //{
            //    columnStart += 1;
            //}

            var ordinal = 1;

            var groups = GetGroupColumnSettings(columnSettings);

            foreach (var t in data)
            {
                var rowIndex = currentRow;
                var r = t;

                var maxCount = 1;
                if (groups.Any())
                {
                    maxCount = groups.Select(g => g.GetRowCount(r)).Max();
                }

                if (maxCount == 0)
                {
                    maxCount = 1;
                }
                if (hasOrdinalColumn)
                {
                    sheet.SetValue(currentRow, 1, ordinal);
                    SetValue(sheet, rowIndex, rowIndex + maxCount - 1, 1, 1, ordinal);
                    ordinal++;
                }

                foreach (var column in columnSettings)
                {
                    //var colNum = columnStart + column.Index;
                    if (column.IsGroup)
                    {
                        if (column is IWriteColumnGroup group)
                        {
                            var values = group.GetValues(r);
                            WriteExcelColumnGroup(sheet, group, currentRow, values, maxCount);
                        }
                    }
                    else
                    {
                        var v = column.GetValue(r);
                        SetValue(sheet, rowIndex, rowIndex + maxCount - 1, column.Index, column.Index, v, column.Format);
                    }
                }
                currentRow += maxCount;
            }

            //格式
            var area = sheet.Cells[startRow, 1, currentRow - 1, columnEnd - 1];
            area.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            area.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            area.Style.WrapText = true;
            area.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            area.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            area.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            area.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            return currentRow;
        }

        /// <summary>
        /// 写入列组
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="columnSetting"></param>
        /// <param name="rowStart"></param>
        /// <param name="data"></param>
        /// <param name="mergeCount">合并行数</param>
        private void WriteExcelColumnGroup(ExcelWorksheet sheet, IWriteColumnGroup columnSetting, int rowStart, IEnumerable<object> data, int mergeCount)
        {
            var columns = columnSetting.GetColumnSettings().ToArray();

            if (data == null)
            {
                foreach (var column in columns)
                {
                    //var colIndex = columnStart + column.Index - 1;
                    SetValue(sheet, rowStart, rowStart + mergeCount - 1, column.Index, column.Index, null);
                }
                return;
            }

            var enumerable = data.ToArray();
            if (columns.Length < 1 && !enumerable.Any())
            {
                return;
            }

            var cellCount = 1;
            if (enumerable.Length == 1 && mergeCount > 1)
            {
                cellCount = mergeCount;
            }
            foreach (var t in enumerable)
            {
                var r = t;
                foreach (var column in columns)
                {
                    var v = column.GetValue(r);
                    SetValue(sheet, rowStart, rowStart + cellCount - 1, column.Index, column.Index, v, column.Format);
                }
                rowStart++;
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowStart"></param>
        /// <param name="rowEnd"></param>
        /// <param name="columnStart"></param>
        /// <param name="columnEnd"></param>
        /// <param name="v"></param>
        /// <param name="format"></param>
        private void SetValue(ExcelWorksheet sheet, int rowStart, int rowEnd, int columnStart, int columnEnd, object v, string format = null)
        {
            if (v != null)
            {
                if (v is long l && string.IsNullOrEmpty(format))
                {
                    sheet.SetValue(rowStart, columnStart, l.ToString());
                }
                else
                {
                    sheet.SetValue(rowStart, columnStart, v);
                }

                if (!string.IsNullOrEmpty(format))
                {
                    sheet.Cells[rowStart, columnStart].Style.Numberformat.Format = format;
                }
                else if (v is DateTime)
                {
                    sheet.Cells[rowStart, columnStart].Style.Numberformat.Format = "yyyy-MM-DD HH:mm:ss";
                }
            }

            if (rowStart != rowEnd || columnStart != columnEnd)
            {
                sheet.Cells[rowStart, columnStart, rowEnd, columnEnd].Merge = true;
            }
        }

        private List<IWriteColumnGroup> GetGroupColumnSettings(IEnumerable<IWriteColumnSetting> columnSettings)
        {
            var settings = columnSettings.Where(c => c.IsGroup).ToList();
            var list = new List<IWriteColumnGroup>();
            foreach (var s in settings)
            {
                if (s is IWriteColumnGroup g)
                {
                    list.Add(g);
                }
            }
            return list;
        }


        public byte[] WriteAndSave<T>(WriteSheetSettings<T> sheetSettings) where T : class
        {
            Write(sheetSettings);
            return Save();
        }

        private void CreateExcelIfNull()
        {
            if (_excelPackage == null)
            {
                _excelPackage = new ExcelPackage();
            }
        }
    }
}
