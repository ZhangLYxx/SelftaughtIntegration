namespace Integration.Excel.ExcelSettings
{
    /// <summary>
    /// Excel 写入接口
    /// </summary>
    public interface IExcelWriter
    {
        /// <summary>
        /// 写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheetSettings"></param>
        void Write<T>(WriteSheetSettings<T> sheetSettings) where T : class;

        /// <summary>
        /// 写入并保存（关闭）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheetSettings"></param>
        /// <returns></returns>
        byte[] WriteAndSave<T>(WriteSheetSettings<T> sheetSettings) where T : class;

        /// <summary>
        /// 保存（关闭）
        /// </summary>
        /// <returns></returns>
        byte[] Save();
    }
}
