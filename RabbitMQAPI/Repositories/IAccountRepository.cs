using Microsoft.AspNetCore.Identity;
using RabbitMQAPI.Models;

namespace RabbitMQAPI.Repositories
{
    public interface IAccountRepository
    {

        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
    }
}
