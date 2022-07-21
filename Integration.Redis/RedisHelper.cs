using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Integration.Redis
{
    /// <summary>
    /// Redis帮助类
    /// </summary>
    public class RedisHelper
    {
        public ConnectionMultiplexer redis { get; set; }
        public IDatabase db { get; set; }
        public RedisHelper(string connection)
        {
            redis = ConnectionMultiplexer.Connect(connection);
            db = redis.GetDatabase();
        }
        /// <summary>
        /// 缓存增加/修改
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set(string key, string value)
        {
            return db.StringSet(key, value);
        }
        /// <summary>
        /// 设置缓存过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Expire(string key,DateTime? time)
        {
            return db.KeyExpire(key, time);
        }
        /// <summary>
        /// 缓存查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return db.StringGet(key);
        }
        /// <summary>
        /// 缓存删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            return db.KeyDelete(key);
        }
    }
}