using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCmember.Dto.Member
{
    /// <summary>
    /// 订单导出记录
    /// </summary>
    public class ExportRecord
    {
        public Guid Id { get; set; }

        public long CreateBy { get; set; }

        public string CreateName { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 保存目录
        /// </summary>
        public string PhysicsPath { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }


    }
}
