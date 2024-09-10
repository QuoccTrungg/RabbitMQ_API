using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client1.Models;

namespace RabbitMQ.Client1.Data
{
    public class DbContextClass : DbContext    {
        protected readonly IConfiguration Configuration;
        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<ServerInfor> ServerInfor
        { get;set; }


    }
}
