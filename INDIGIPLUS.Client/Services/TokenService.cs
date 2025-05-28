using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;

namespace INDIGIPLUS.Client.Services
{
    public class TokenService
    {
        private readonly ILocalStorageService _localStorage;
        private const string TokenKey = "authToken";

        public TokenService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SetTokenAsync(string token)
        {
            try
            {
                await _localStorage.SetItemAsync(TokenKey, token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting token: {ex.Message}");
            }
        }

        public async Task<string?> GetTokenAsync()
        {
            try
            {
                return await _localStorage.GetItemAsync<string>(TokenKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting token: {ex.Message}");
                return null;
            }
        }

        public async Task RemoveTokenAsync()
        {
            try
            {
                await _localStorage.RemoveItemAsync(TokenKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing token: {ex.Message}");
            }
        }

        public bool IsTokenExpired(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                return jwtToken.ValidTo <= DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking token expiration: {ex.Message}");
                return true; // Assume expired if we can't parse it
            }
        }
    }
}