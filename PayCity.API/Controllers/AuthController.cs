using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PayCity.API.Controllers;
using Microsoft.EntityFrameworkCore;
using PayCity.API.Models;
using PayCity.API.Services;
using PayCity.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PayCity.API.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        //private object _context;

        private readonly AppDbContext _context;
        private User user;

        public object BCrypt { get; private set; }

        public AuthController(IAuthService authService, IConfiguration config)
        {
            _authService = authService;
            _config = config;
        }



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
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [Authorize]
[HttpDelete("delete")]
public async Task<IActionResult> DeleteUser()
{
    // 1. Get the current user's ID from the JWT claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("No user ID found in token.");

            // 2. Convert userId to Guid and find the user in the database
            if (!Guid.TryParse(userId, out Guid guidId))
                return BadRequest("Invalid user ID.");

            var user = await _context.Users.FindAsync(guidId);
            if (user == null)
                return NotFound("User not found.");

            // 3. Delete the user and save changes
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully." });
}

    }
}
