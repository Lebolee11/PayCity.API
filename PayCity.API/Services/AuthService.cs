using PayCity.API.Data;
using PayCity.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace PayCity.API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return false;

            var passwordHash = HashPassword(request.Password);

            var user = new User
            {
                Name = request.Name,
               /* Surname = request.Surname,
                Email = request.Email,
                Phone = request.Phone,
                PasswordHash = passwordHash,
                TermsAccepted = request.TermsAccepted*/
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
