using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.Client1.RabbitMQ
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        public void SendServerInforMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/"
        };
            var connection = factory.CreateConnection();
            
            var channel = connection.CreateModel();
            channel.QueueDeclare("ServerInfor", durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: "ServerInfor", body: body);
        }
    }
}
