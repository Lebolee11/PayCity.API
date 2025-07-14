using PayCity.API.Data;
using Microsoft.EntityFrameworkCore;
//using PayCity.API.Controllers;
using PayCity.API.Services;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services and controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// In Program.cs, inside the builder.Services section
builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<AuthService>();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
