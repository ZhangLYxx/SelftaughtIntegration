using Integration.Excel.ExcelSettings;
using System.Collections.Generic;

namespace UCmember.Dto.Member
{
    /// <summary>
    /// 物流信息导入模型
    /// </summary>
    public class ExpressImportDto : ExpressDto, IReadData
    {

        public ExpressImportDto()
        {
            ErrorMessages = new List<string>();
        }

        public int RowNumber { get; set; }

        /*
        /// <summary>
        /// 订单编号
        /// </summary>
        public string Code { get; set; }
        */

        public long Id { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public List<string> ErrorMessages { get; set; }


        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsOk => ErrorMessages == null || ErrorMessages.Count == 0;

    }
}
