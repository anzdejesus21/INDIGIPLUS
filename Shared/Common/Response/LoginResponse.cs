namespace Shared.Common.Response
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public UserInfo? User { get; set; }
    }
}
