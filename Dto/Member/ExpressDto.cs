using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCmember.Dto.Member
{
    /// <summary>
    /// 物流信息
    /// </summary>
    public class ExpressDto
    {

        /// <summary>
        /// 快递单号
        /// </summary>
        [Required]
        public string TraceCode { get; set; }

        /// <summary>
        /// 快递方式
        /// </summary>
        [Required]
        public string DeliveryMethod { get; set; }

        /*
        /// <summary>
        /// 费用 (未使用)
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// 状态 （未使用）
        /// </summary>
        public int State { get; set; }
        */
    }
}
