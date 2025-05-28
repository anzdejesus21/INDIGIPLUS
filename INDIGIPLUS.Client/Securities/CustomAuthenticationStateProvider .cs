using INDIGIPLUS.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace INDIGIPLUS.Client.Securities
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly TokenService _tokenService;

        public CustomAuthenticationStateProvider(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _tokenService.GetTokenAsync();

                if (string.IsNullOrEmpty(token) || _tokenService.IsTokenExpired(token))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var claims = ParseClaimsFromJwt(token);
                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting authentication state: {ex.Message}");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);
                return token.Claims;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing JWT claims: {ex.Message}");
                return new List<Claim>();
            }
        }
    }
}