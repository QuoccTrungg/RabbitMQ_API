using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RabbitMQAPI.Data;
using RabbitMQAPI.Repositories;
using System.Text;

var MyAllowSpecificOrigins = "https://localhost:44387/api/ServerInfor";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7052");
                      });
});*/

builder.Services.AddScoped<IAccountRepository,AccountRepository>();

builder.Services.AddCors(Options => {
    Options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MyDBContext>().AddDefaultTokenProviders();   
builder.Services.AddControllers();
builder.Services.AddDbContext<MyDBContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));
} );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(Options =>
{
    Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(Options => { Options.SaveToken = true;
Options.RequireHttpsMetadata = false;
Options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters { 
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidAudience = builder.Configuration["JWT:ValidAudience"],
    ValidIssuer= builder.Configuration["JWT:ValidIssuer"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
};
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
/*app.UseCors(MyAllowSpecificOrigins);*/
app.UseCors();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();


app.Run();
