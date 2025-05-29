using INDIGIPLUS.Api.Common.Response;
using INDIGIPLUS.Api.DTOs.Auth;
using INDIGIPLUS.Api.Entities;

namespace INDIGIPLUS.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> ValidateTokenAsync(string token);
        string GenerateJwtToken(User user);
    }
}
