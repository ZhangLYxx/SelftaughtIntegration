using System.Collections.Generic;

namespace Integration.Excel.ExcelSettings
{
    public interface IReadData
    {
        /// <summary>
        /// 行号
        /// </summary>
        int RowNumber { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        List<string> ErrorMessages { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        bool IsOk { get; }
    }
}
