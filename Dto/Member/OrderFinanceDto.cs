using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCmember.Dto.Member
{
    public class OrderFinanceDto
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 详情id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public decimal? SalePrice { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayAmount { get; set; }
    }

    public class PayRecordExportDto
    {
        public string Code { get; set; }

        /// <summary>
        /// 嘉弦积分
        /// </summary>
        public decimal JXPoints { get; set; }

        /// <summary>
        /// 微信支付
        /// </summary>
        public decimal WxPrice { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal ExpressFee { get; set; }

        public List<OrderFinanceDto> OrderFinances { get; set; }
    }
}
