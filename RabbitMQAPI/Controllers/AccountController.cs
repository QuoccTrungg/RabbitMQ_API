using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQAPI.Models;
using RabbitMQAPI.Repositories;

namespace RabbitMQAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepo;

        public AccountController(IAccountRepository repo) {
            accountRepo = repo;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel model) { 
        
            var result = await accountRepo.SignUpAsync(model);
            Console.WriteLine(result);
            if (result.Succeeded) {
                return Ok(result.Succeeded);
                           }
             return Unauthorized(); 
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel model) {
            var result = await accountRepo.SignInAsync(model);
            if (string.IsNullOrEmpty(result))
                return Unauthorized();
            return Ok(result);
        }
    }
}
