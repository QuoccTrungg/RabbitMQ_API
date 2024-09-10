using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RabbitMQAPI.Data;
using RabbitMQAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RabbitMQAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public  AccountRepository(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }
        public async Task<string> SignInAsync(SignInModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
             if(!result.Succeeded)
                return string.Empty;
            var authClaims = new List<Claim> { 
                new Claim(ClaimTypes.Email,model.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };

                var authKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var Token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey,SecurityAlgorithms.HmacSha512Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
            }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {

            var user = new ApplicationUser {
                Fistname = model.Fistname,
                Lastname = model.Lastname,
                Email = model.Email,
                UserName = model.Email,

            };
            return await userManager.CreateAsync(user,model.Password);
        }
    }
}
