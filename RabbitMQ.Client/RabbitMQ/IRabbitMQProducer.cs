namespace RabbitMQ.Client1.RabbitMQ
{
    public interface IRabbitMQProducer
    {
        public void SendServerInforMessage<T>(T message);
    }
}
