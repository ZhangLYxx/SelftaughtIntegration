using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.ToolKits
{
    public class PageData<T>
    {
        public PageData()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="records">数据</param>
        /// <param name="total">总行数</param>
        public PageData(IEnumerable<T> records, int total)
        {
            Records = records;
            Total = total;
        }

        /// <summary>
        /// 总行数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 记录
        /// </summary>
        public IEnumerable<T> Records { get; set; }
    }
}
