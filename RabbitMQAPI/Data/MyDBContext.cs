using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace RabbitMQAPI.Data
{
    public class MyDBContext :IdentityDbContext<ApplicationUser>
    {
        public MyDBContext(DbContextOptions<MyDBContext> options): base(options) { }
        public DbSet<ServerInfor>? serverInfors { get; set; }
       // public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
