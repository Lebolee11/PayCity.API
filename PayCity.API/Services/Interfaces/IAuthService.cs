public interface IAuthService
    {
        Task<string?> LoginAsync(string email, string password);
        Task SendResetEmail(string email);
        Task<bool> RegisterAsync(RegisterRequest request);
    }