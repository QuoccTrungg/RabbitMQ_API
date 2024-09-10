using Microsoft.AspNetCore.Identity;

namespace RabbitMQAPI.Data
{
    public class ApplicationUser: IdentityUser
    {
        public string Fistname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
    }
}
