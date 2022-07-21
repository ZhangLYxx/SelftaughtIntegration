using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Redis
{
    /// <summary>
    /// 返回结果类
    /// </summary>
    public class RedisResult
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        public bool ImplementationResults { get; set; }
        /// <summary>
        /// Value的实时值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 缓存的Value值
        /// </summary>
        public string CacheValue { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }
    }
}
