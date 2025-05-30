using INDIGIPLUS.Client.Common.Response;
using INDIGIPLUS.Client.DTOs.Auth;
using INDIGIPLUS.Client.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace INDIGIPLUS.Client.Services
{
    public class AuthClientService : IAuthClientService
    {
        #region Fields

        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;
        private string _currentToken = string.Empty;
        private UserInfo? _currentUser;

        #endregion Fields

        #region Public Constructors

        public AuthClientService(HttpClient httpClient, TokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/auth/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // Enhanced error handling
                    Console.WriteLine($"HTTP Error - Status: {response.StatusCode}, Reason: {response.ReasonPhrase}");

                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        return errorResponse ?? new LoginResponse { Success = false, Message = $"HTTP {response.StatusCode}: {response.ReasonPhrase} - {responseContent}" };
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"JSON Deserialization Error: {jsonEx.Message}");
                        return new LoginResponse { Success = false, Message = $"HTTP {response.StatusCode}: {response.ReasonPhrase} - Raw Response: {responseContent}" };
                    }
                }

                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (loginResponse?.Success == true && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    Console.WriteLine("Setting token and updating HttpClient headers");
                    _currentToken = loginResponse.Token;
                    _currentUser = loginResponse.User;
                    await _tokenService.SetTokenAsync(loginResponse.Token);

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
                    Console.WriteLine("Token set successfully");
                }

                return loginResponse ?? new LoginResponse { Success = false, Message = "Invalid response format" };
            }
            catch (HttpRequestException httpEx)
            {
                return new LoginResponse { Success = false, Message = $"Network error: {httpEx.Message}" };
            }
            catch (TaskCanceledException tcEx)
            {
                Console.WriteLine($"Request Timeout: {tcEx.Message}");
                return new LoginResponse { Success = false, Message = "Request timeout - please try again" };
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON Exception: {jsonEx.Message}");
                return new LoginResponse { Success = false, Message = $"Response parsing error: {jsonEx.Message}" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Exception: {ex.Message}");
                Console.WriteLine($"Login Stack Trace: {ex.StackTrace}");
                return new LoginResponse { Success = false, Message = $"Login failed: {ex.Message}" };
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                Console.WriteLine($"Register attempt for email: {request.Email}");

                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                Console.WriteLine($"Register Request JSON: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/auth/register", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Register Response Status: {response.StatusCode}");
                Console.WriteLine($"Register Response Content: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<RegisterResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        return errorResponse ?? new RegisterResponse { Success = false, Message = $"HTTP {response.StatusCode}: {response.ReasonPhrase} - {responseContent}" };
                    }
                    catch (JsonException)
                    {
                        return new RegisterResponse { Success = false, Message = $"HTTP {response.StatusCode}: {response.ReasonPhrase} - Raw Response: {responseContent}" };
                    }
                }

                var registerResponse = JsonSerializer.Deserialize<RegisterResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return registerResponse ?? new RegisterResponse { Success = false, Message = "Invalid response format" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Register Exception: {ex.Message}");
                Console.WriteLine($"Register Stack Trace: {ex.StackTrace}");
                return new RegisterResponse { Success = false, Message = $"Registration failed: {ex.Message}" };
            }
        }

        public async Task<UserInfo?> GetCurrentUserAsync()
        {
            if (_currentUser != null)
                return _currentUser;

            try
            {
                var token = await _tokenService.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("No token found in GetCurrentUserAsync");
                    return null;
                }

                Console.WriteLine($"Getting current user with token: {token.Substring(0, Math.Min(20, token.Length))}...");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("/api/auth/user");
                Console.WriteLine($"GetCurrentUser Response Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"GetCurrentUser Response Content: {responseContent}");

                    _currentUser = JsonSerializer.Deserialize<UserInfo>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"GetCurrentUser Error: {response.StatusCode} - {errorContent}");
                }

                return _currentUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetCurrentUser Exception: {ex.Message}");
                Console.WriteLine($"GetCurrentUser Stack Trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task LogoutAsync()
        {
            Console.WriteLine("Performing logout");
            _currentToken = string.Empty;
            _currentUser = null;
            await _tokenService.RemoveTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            Console.WriteLine("Logout completed");
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                var token = await _tokenService.GetTokenAsync();

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("No token found in IsAuthenticatedAsync");
                    return false;
                }

                if (_tokenService.IsTokenExpired(token))
                {
                    Console.WriteLine("Token is expired, logging out");
                    await LogoutAsync();
                    return false;
                }

                Console.WriteLine("User is authenticated");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IsAuthenticatedAsync Exception: {ex.Message}");
                return false;
            }
        }

        #endregion Public Methods
    }
}