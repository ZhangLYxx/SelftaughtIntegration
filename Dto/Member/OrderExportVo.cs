using System;


namespace UCmember.Dto.Member
{
    /// <summary>
    /// 订单导出模型
    /// </summary>
    public class OrderExportVo
    {
        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 详情id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 用户备注
        /// </summary>
        public string MemberRemark { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string ContactName { get; set; }

        /*
        /// <summary>
        /// 来源
        /// </summary>
        public string ChannelName { get; set; }
        */

        /// <summary>
        /// 来源
        /// </summary>
        public string CardDescription { get; set; }


        /// <summary>
        /// 快递公司
        /// </summary>
        public string DeliveryCompany { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string DeliveryCode { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public long OrderId { get; set; }

    }
}
