using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCmember.Dto.Member
{
    public class FianceQueryDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginCreateTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndCreateTime { get; set; }
    }
}
