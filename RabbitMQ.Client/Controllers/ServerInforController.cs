using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client1.Data;
using RabbitMQ.Client1.Models;
using RabbitMQ.Client1.RabbitMQ;
using RabbitMQ.Client1.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace RabbitMQ.Client1.Controllers
{
    public class ServerInforController : ControllerBase { 
        private readonly IServerInforService serverInforService;
        private readonly IRabbitMQProducer _rabitMQProducer;
        private readonly DbContextClass _dbContext;
        public PerformanceCounter DCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
        public PerformanceCounter CPUCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        public PerformanceCounter RAMCounter = new PerformanceCounter("Memory", "Available MBytes", null);
        public ServerInforController(IServerInforService _serverInforService, IRabbitMQProducer rabitMQProducer, DbContextClass dbContext)
        {
            serverInforService = _serverInforService;
            _rabitMQProducer = rabitMQProducer;
            _dbContext = dbContext;
        }
        
        
            
       
        [HttpGet("serverInforlist")]
        public IActionResult ServerInforList()
        {
            var serverInforList = _dbContext.ServerInfor.ToList();
            return Ok(serverInforList);
        }

        [HttpGet("getserverInforbyid")]
        public ServerInfor GetServerInforById(int Id)
        {
            return serverInforService.GetServerInforById(Id);
        }
        [HttpPost("addserverInfor")]
        public IActionResult AddServerInfor(ServerInforModel model)
        {   try {
                var newsvi = new ServerInfor {
                    
                    Name = model.Name,
                    CPU = model.CPU,
                    RAM = Math.Round(RAMCounter.NextValue()/160,2),
                    D = model.D,               
                    Time = DateTime.Now,
                    };
                _dbContext.Add(newsvi);
                _dbContext.SaveChanges();
                //send the inserted product data to the queue and consumer will listening this data from queue
                _rabitMQProducer.SendServerInforMessage(newsvi);
                return Ok( newsvi);
            }
            
            catch
            {
                return BadRequest();
            }
  
        }
        [HttpPut("updateserverInfor")]
        public ServerInfor UpdateServerInfor(ServerInfor serverInfor)
        {
            return serverInforService.UpdateServerInfor(serverInfor);
        }
        [HttpDelete("deleteserverInfor")]
        public bool DeleteServerInfor(int Id)
        {
            return serverInforService.DeleteServerInfor(Id);
        }
    }
}
