using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PayCity.API.Controllers;

namespace PayCity.API.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            if (token == null) return Unauthorized();
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            return result ? Ok(new { message = "Registration successful" }) : BadRequest();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _authService.SendResetEmail(request.Email);
            return Ok(new { message = "Password reset link sent" });
        }
    }

    public interface IAuthService
    {
        Task<string?> LoginAsync(string email, string password);
        Task SendResetEmail(string email);
        Task<bool> RegisterAsync(RegisterRequest request);
    }
}
