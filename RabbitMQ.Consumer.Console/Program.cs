// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client1.Data;
using RabbitMQ.Client1.Models;
using System.Text;
using RabbitMQ.Client1.Data;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Net.Mail;
using System.Net;
using Microsoft.Data.SqlClient;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection.Metadata;
using System.Drawing;
using System.Runtime.Intrinsics.Arm;
using System.Data.Common;
using Dapper;



//Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
var factory = new ConnectionFactory
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest",
    VirtualHost = "/"
};
//Create the RabbitMQ connection using connection factory details as i mentioned above
var connection = factory.CreateConnection();
//Here we create channel with session and model
using
var channel = connection.CreateModel();
//declare the queue after mentioning name and a few property related to that
channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);

var QueueName= channel.QueueDeclare().QueueName;

//Set Event object which listen message from chanel which is sent by producer
var consumer = new EventingBasicConsumer(channel);
channel.QueueBind(queue:QueueName,exchange: "pubsub",routingKey: "analyticsonly");
consumer.Received += (model, eventArgs) => {
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine("Product message received:" +message);
    var chunks = message.Split("|");
    var svinew = new ServerInforModel();
    if(chunks.Length == 7)
    {
        
        svinew.Name = chunks[0];
        svinew.CPU = Convert.ToDouble(chunks[1]);
        svinew.RAM = Convert.ToDouble(chunks[2]);
        svinew.D = Convert.ToDouble(chunks[3]);
        svinew.Time = DateTime.Now;
        svinew.IP = chunks[5];
        svinew.Region = chunks[6];
        Console.WriteLine(svinew.Name);

    }
    DataTable check = DataProvider.Instance.ExecuteQuery("select * from ServerInfor where Name ='"+svinew.Name+"'");
    if (check != null) {
        int insert = DataProvider.Instance.ExecuteNonQuery("INSERT INTO ServerInfor (Name, CPU, RAM, D,IP,Region)  VALUES ('" + svinew.Name+"',"+ svinew.CPU + "," + svinew.RAM + ","+ svinew.D+ ",'" + svinew.IP + "','" + svinew.Region + "');");
        /*string connectionStr = "data Source=LAPTOP-UEL9U0VQ;initial catalog=RabbitMQ;user id=sa;password=123";
        using (SqlConnection connection = new SqlConnection(connectionStr))
        {   
            connection.Open();
            connection.Execute(@"
            "INSERT INTO ServerInfor (Name, CPU, RAM, D,IP,Region)  VALUES ('" + svinew.Name+"',"+ svinew.CPU + "," + svinew.RAM + ","+ svinew.D+ "," + svinew.IP + "," + svinew.Region + "),
         new { name = dep.DepartmentName, id = dep.DepartmentId });
            connection.Close();
        }*/
        if (svinew.CPU > 50 || svinew.RAM > 50 || svinew.D > 50) {
            string from, to, pass, content = "";
            from = "trungngo2901@gmail.com";
            //from = "n19dccn215@student.ptithcm.edu.vn";
            pass = "grqoffbudwccltes";
            to = "trungngo15t@gmail.com";

            content += " ServerName " + svinew.Name + "\t\t\t" + " CPU: " + svinew.CPU + "\t\t\t" + " RAM: " + svinew.RAM + "\t\t\t"
                   + "  Disk: " + svinew.D + "\t\t\t" + "  Time: " + svinew.Time.ToString() + "\t\t\t" + "  IP: " + svinew.IP + "\t\t\t" + "  Region: " + svinew.Region + "\n";

            if (svinew.CPU > 50) content += "Warning " + " Server " + svinew.Name + "!!! CPU : " + svinew.CPU + "\n";
            if (svinew.RAM > 50) content += "Warning " + " Server " + svinew.Name + "!!!  RAM : " + svinew.RAM + "\n";
            if(svinew.D > 50) content += "Warning " + " Server " + svinew.Name + "!!!  Disk : " + svinew.D + "\n";
            //content = sv.ToString();
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(from);
            mail.Subject = " Warning Server " + svinew.Name + " Time: " + DateTime.Now.ToString();
            mail.Body = content;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);
            try
            {
                smtp.Send(mail);             
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }

        }
    }
};
//read the message
channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
Console.ReadKey();