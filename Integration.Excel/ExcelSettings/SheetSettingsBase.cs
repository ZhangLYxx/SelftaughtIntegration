namespace Integration.Excel.ExcelSettings
{
    public abstract class SheetSettingsBase<T> where T : class
    {
        /// <summary>
        /// 工作表名称
        /// </summary>
        public string SheetName { get; set; }


        /// <summary>
        /// 是否有 序号 列
        /// </summary>
        public bool HasOrdinalColumn { get; set; }

    }
}
