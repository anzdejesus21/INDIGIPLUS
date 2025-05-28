using Shared.Common.Response;
using Shared.DTOs;


namespace INDIGIPLUS.Client.Services.Interfaces
{
    public interface IAuthClientService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<UserInfo?> GetCurrentUserAsync();
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
    }
}
