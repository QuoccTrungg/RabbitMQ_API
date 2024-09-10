using System.ComponentModel.DataAnnotations;

namespace RabbitMQAPI.Models
{
    public class SignUpModel
    {
        [Required]
        public string Fistname { get; set; } = null!;
        [Required]
        public string Lastname { get; set; } = null!;
        [Required,EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string ConfirmPassword { get; set; } = null!;
    }
}
