using Integration.EntityFrameworkCore.DbMigrations.SqlServer;
using Integration.Redis.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Integration.Redis.WebApi.Controllers
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    [Route("/api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MigrationsDbContext _dbContext;

        public RedisController(IConfiguration configuration, MigrationsDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        #region Redis基础操作
        /// <summary>
        /// 链接
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private RedisHelper Link(int point)
        {
            string? link;
            switch (point)
            {
                case 2:
                    link = _configuration.GetValue<string>("Redis:link2");
                    break;
                case 3:
                    link = _configuration.GetValue<string>("Redis:link3");
                    break;
                default:
                    link = _configuration.GetValue<string>("Redis:link1");
                    break;
            }

            return new RedisHelper(link);
        }


        /// <summary>
        /// 缓存查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public string Search(string key)
        {
            string returnStr = string.Empty;
            if (!string.IsNullOrWhiteSpace(key))
            {
                var value = Link(1).Get(key);
                if (!string.IsNullOrWhiteSpace(value))
                    returnStr = value;
            }
            else
                returnStr = "key的值不能为空！";
            return returnStr;
        }

        /// <summary>
        /// 缓存时间节点
        /// </summary>
        public enum TimeNode
        {
            Seconds,
            Minutes,
            Hours,
            Day
        }

        /// <summary>
        /// 缓存时间转换
        /// </summary>
        /// <param name="time"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private DateTime? TransTime(TimeNode? time, int count)
        {
            if (time.HasValue)
            {
                switch ((int)time)
                {
                    case 0:
                        return DateTime.Now.AddSeconds(count);
                    case 1:
                        return DateTime.Now.AddMinutes(count);
                    case 2:
                        return DateTime.Now.AddHours(count);
                    case 3:
                        return DateTime.Now.AddDays(count);
                    default:
                        return null;
                }
            }
            return null;
        }

        /// <summary>
        /// 新增缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPost]
        public RedisResult Insert(string key, string value, TimeNode? time = null, int count = 0)
        {
            var result = new RedisResult();
            if (!string.IsNullOrWhiteSpace(key))
            {
                var tn = TransTime(time, count);
                bool isInsertSuccess = Link(2).Set(key, value);
                result.ImplementationResults = isInsertSuccess;
                if (isInsertSuccess)
                {
                    var info = Search(key);
                    if (!string.IsNullOrWhiteSpace(info))
                    {
                        result.Value = info;
                        var isExpireSuccess = Link(2).Expire(key, tn);
                        if (!isExpireSuccess)
                        {
                            result.Error = "设置缓存过期时间失败";
                        }
                    }
                }
                else
                {
                    result.Error = "设置缓存失败";
                }
            }
            else
                result.Error = "key的值不能为空！";
            return result;
        }

        /// <summary>
        /// 缓存删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpDelete]
        public RedisResult Delete(string key)
        {
            var result = new RedisResult();
            if (!string.IsNullOrWhiteSpace(key))
            {
                result.Value = Search(key);
                bool isInsertSuccess = Link(3).Delete(key);
                result.ImplementationResults = isInsertSuccess;
            }
            else
                result.Error = "key的值不能为空！";
            return result;
        }


        #endregion

        #region Cache Aside(旁路缓存)

        /// <summary>
        /// 读策略
        /// 如果读取的数据命中了缓存，则直接返回数据；
        /// 如果读取的数据没有命中缓存，则从数据库中读取数据，然后将数据写入到缓存，并且返回给用户。
        /// 适合读多写少的场景，不适合写多的场景
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public async Task<RedisRowDto> ReadByAside(long id)
        {
            var result = Search(id.ToString());
            if (!string.IsNullOrWhiteSpace(result))
            {
                var re = JsonConvert.DeserializeObject<RedisRowDto>(result);
                return re;
            }
            var x = await _dbContext.Members.FirstOrDefaultAsync(c => c.Id == id);
            var s = new RedisRowDto()
            {
                Id = x.Id,
                Age = x.Age,
                Birthday = x.Birthday,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber
            };
            var rt = JsonConvert.SerializeObject(s);
            var IsInsertSuccess = Insert(id.ToString(), rt);
            if (IsInsertSuccess.ImplementationResults)
            {
                return s;
            }
            return null;
        }

        /// <summary>
        /// 写策略
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<bool> WriteByAside(long id, [FromBody] RedisDetailDto dto)
        {
            var query = await _dbContext.Members.FirstOrDefaultAsync(c => c.Id == id);
            if (query != null)
            {
                query.Name=dto.Name;
                query.Age=dto.Age;
                query.Birthday=dto.Birthday;
                query.PhoneNumber=dto.PhoneNumber;
                await _dbContext.SaveChangesAsync();
                var re = Delete(id.ToString());
                return re.ImplementationResults;
            }

            return false;
        }

        #endregion

        #region Read/Write Through（读穿 / 写穿）策略
        #endregion
    }
}
