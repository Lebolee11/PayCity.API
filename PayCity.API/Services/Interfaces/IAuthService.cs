using PayCity.API.Models;

public interface IAuthService
{
    Task<string?> LoginAsync(string email, string password);
    Task SendResetEmail(string email);
    Task<bool> RegisterAsync(RegisterRequest request);
    Task<bool> RegistersAsync(RegisterRequest request);
}