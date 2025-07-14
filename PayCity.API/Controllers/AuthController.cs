using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PayCity.API.Controllers;
using Microsoft.EntityFrameworkCore;
using PayCity.API.Models;
using PayCity.API.Services;
using PayCity.API.Data;

namespace PayCity.API.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] PayCity.API.Models.RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
                return BadRequest(new { message = "Passwords do not match." });

            var result = await _authService.RegisterAsync(request);
            if (!result)
                return BadRequest(new { message = "Email already exists." });

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _authService.SendResetEmail(request.Email);
            return Ok(new { message = "Password reset link sent" });
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Microsoft.AspNetCore.Identity.Data.LoginRequest request)
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            if (token == null) return Unauthorized();
            return Ok(new { token });
        }

        [HttpGet("users")]
public async Task<IActionResult> GetAllUsers([FromServices] AppDbContext context)
{
    var users = await context.Users.ToListAsync();
    return Ok(users);
}

[HttpGet("users/{id}")]
public async Task<IActionResult> GetUserById(int id, [FromServices] AppDbContext context)
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return NotFound();
    return Ok(user);
}
    }

    
}
