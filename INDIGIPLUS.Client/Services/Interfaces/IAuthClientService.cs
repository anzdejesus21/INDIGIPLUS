using INDIGIPLUS.Client.Common.Response;
using INDIGIPLUS.Client.DTOs;

namespace INDIGIPLUS.Client.Services.Interfaces
{
    public interface IAuthClientService
    {
        #region Public Methods

        Task<LoginResponse> LoginAsync(LoginRequest request);

        Task<RegisterResponse> RegisterAsync(RegisterRequest request);

        Task<UserInfo?> GetCurrentUserAsync();

        Task LogoutAsync();

        Task<bool> IsAuthenticatedAsync();

        #endregion Public Methods
    }
}