using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client1.Models;

namespace RabbitMQ.Client1.Services
{
    public interface IServerInforService
    {
        public IEnumerable<ServerInfor> GetServerInforList();
        public ServerInfor GetServerInforById(int id);
        public ServerInfor AddServerInfor(ServerInfor ServerInfor);
        public ServerInfor UpdateServerInfor(ServerInfor ServerInfor);
        public bool DeleteServerInfor(int Id);
      
    }
}
