// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client1.Models;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Text;
using System.Xml.Linq;


var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
var RAMCounter = new PerformanceCounter("Memory", "Available MBytes", null);
var DCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
var factory = new ConnectionFactory
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest",
    VirtualHost = "/"
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);
channel.QueueDeclare("ServerInfor", durable: false,
exclusive: false,
autoDelete: false,
arguments: null);
int index = 1;
while (index >0)
{
    var svi = new ServerInfor
    {
        id = 1,
        Name = "SV2",
        CPU = Math.Round(cpuCounter.NextValue(), 2),
        RAM = Math.Round(RAMCounter.NextValue() / 160, 2),
        D = Math.Round(DCounter.NextValue(), 2),
        Time = DateTime.Now,
        IP = "192.168.1.2",
        Region = "Đà Nẵng"
    };
    string message = $"{svi.Name}|{svi.CPU}|{svi.RAM}|{svi.D}|{svi.Time}|{svi.IP}|{svi.Region}";
    //var json = JsonConvert.SerializeObject(message);
    //var body = Encoding.UTF8.GetBytes(json);
    var body = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchange: "pubsub", routingKey: "analyticsonly", body: body);
    Console.WriteLine(" [x] Sent {0}", message + " count :" + index);

    index++; Thread.Sleep(5000);
}
