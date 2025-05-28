namespace INDIGIPLUS.Client.Common.Response
{
    public class LoginResponse
    {
        #region Properties

        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public UserInfo? User { get; set; }

        #endregion Properties
    }
}