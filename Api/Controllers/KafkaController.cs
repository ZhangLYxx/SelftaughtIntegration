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

        [HttpGet("[action]")]
        public int G()
        {
            var c = new string[] { "ccc", "ssssss", "vvv" };
            var s = c.Count(v => v.Contains("ccc"));
            return s;
        }


        [HttpPost]
        public async Task<string> Production([FromBody] ProducerDto dto)
        {
            await _kafkaService.PublishAsync(dto.TopicName, dto.Message);
            return $"生成topic:{dto.TopicName},message:{dto.Message}成功";
        }
        #endregion

        #region 消费者
        [HttpGet]
        public Task<SubacribeData> Consumption([FromQuery] ConsumptionQueryDto dto)
        {
            var result = _kafkaService.Subscribe(dto.TopicName);
            return result;
        }
        #endregion
    }
}
