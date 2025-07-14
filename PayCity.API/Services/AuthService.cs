using PayCity.API.Data;
using PayCity.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace PayCity.API.Services
{
    
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            var passwordHash = HashPassword(password);
            if (user.PasswordHash != passwordHash) return null;

            // For demo: return a dummy token
            return "dummy-jwt-token";
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
                return false; // Or throw a custom exception or return a specific error

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return false;

            var passwordHash = HashPassword(request.Password);

            var user = new User
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                PasswordHash = passwordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> RegistersAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task SendResetEmail(string email)
        {
            // For demo: pretend to send an email
            return Task.CompletedTask;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
