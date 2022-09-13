using System;

namespace UCmember.Dto.Member
{
    /// <summary>
    /// 订单导出模型
    /// </summary>
    public class OrderExportDto
    {
        /// <summary>
        /// 下单时间 （起）
        /// </summary>
        public DateTime? BeginCreateTime { get; set; }

        /// <summary>
        /// 下单时间 （止）
        /// </summary>
        public DateTime? EndCreateTime { get; set; }

        /// <summary>
        /// 渠道编号
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int[] States { get; set; }

        /// <summary>
        /// 关键词字段
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 订单类型（0：兑换订单，1：商城订单）
        /// </summary>
        public int? Type { get; set; }

    }
}
