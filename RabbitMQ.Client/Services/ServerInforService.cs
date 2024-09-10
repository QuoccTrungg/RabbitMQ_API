using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client1.Data;
using RabbitMQ.Client1.Models;

namespace RabbitMQ.Client1.Services
{
    public class ServerInforService : IServerInforService
    {
        private readonly DbContextClass _dbContext;
        public ServerInforService(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }
        public ServerInfor AddServerInfor(ServerInfor ServerInfor)
        {
            var result = _dbContext.ServerInfor.Add(ServerInfor);
            _dbContext.SaveChanges();
            return result.Entity;
        }

        public bool DeleteServerInfor(int Id)
        {
            var filteredData = _dbContext.ServerInfor.Where(x => x.id == Id).FirstOrDefault();
            var result = _dbContext.Remove(filteredData);
            _dbContext.SaveChanges();
            return result != null ? true : false;
        }

        public ServerInfor GetServerInforById(int id)
        {
            return _dbContext.ServerInfor.Where(x => x.id == id).FirstOrDefault();
        }

        public IEnumerable<ServerInfor> GetServerInforList()
        {
            return  _dbContext.ServerInfor.ToList();  
        }

        public ServerInfor UpdateServerInfor(ServerInfor ServerInfor)
        {
            var result = _dbContext.ServerInfor.Update(ServerInfor);
            _dbContext.SaveChanges();
            return result.Entity;
        }


    }
}
