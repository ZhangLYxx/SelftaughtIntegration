using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Kafka
{
    public class KafkaService : IKafkaService
    {
        public async Task PublishAsync(string topicName, string message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9096",
                ClientId = Dns.GetHostName(),
            };
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                string topic = topicName;

                var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
            }
            await Task.CompletedTask;
        }

        public Task<SubacribeData> Subscribe(string topicName)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9096",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            SubacribeData sd = new SubacribeData();
            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                var topic = topicName;
                consumer.Subscribe(topic);

                var consumeResult = consumer.Consume(CancellationToken.None);
                sd.data = consumeResult.Message.Value.ToString();

                consumer.Close();
            }
            return Task.FromResult(sd);
        }
    }
}
