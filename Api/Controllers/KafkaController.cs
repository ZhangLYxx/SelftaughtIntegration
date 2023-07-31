using Integration.JWT;
using Integration.Kafka;
using Microsoft.AspNetCore.Mvc;
using UCmember.Dto.Kafka;

namespace UCmember.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaController : ControllerBase
    {
        private readonly IKafkaService _kafkaService;
        private readonly ILogger<KafkaController> _logger;

        public KafkaController(IKafkaService kafkaService, ILogger<KafkaController> logger)
        {
            _kafkaService = kafkaService;
            _logger = logger;
        }

        #region 生产者
        /// <summary>
        /// 生产者
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Production([FromBody] ProducerDto dto)
        {
            await _kafkaService.PublishAsync(dto.TopicName, dto.Message);
            _logger.LogInformation($"成功");
            return $"生成topic:{dto.TopicName},message:{dto.Message}成功";
        }
        #endregion

        #region 消费者
        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<SubacribeData> Consumption([FromQuery] ConsumptionQueryDto dto)
        {
            var result = _kafkaService.Subscribe(dto.TopicName);
            return result;
        }
        #endregion


        
    }
}
