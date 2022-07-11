namespace Integration.Kafka
{
    public interface IKafkaService
    {
        /// <summary>
        /// 发送消息至指定主题
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="topicName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task PublishAsync(string topicName, string message);

        /// <summary>
        /// 从指定主题订阅消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="topics"></param>
        /// <param name="messageFunc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SubacribeData> Subscribe(string topicName);
    }
}