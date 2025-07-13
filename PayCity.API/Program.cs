using PayCity.API.Data;
using Microsoft.EntityFrameworkCore;
using PayCity.API.Controllers;
using PayCity.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services and controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// In Program.cs, inside the builder.Services section
//builder.Services.AddScoped<IAuthService>();
builder.Services.AddScoped<AuthService>();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
