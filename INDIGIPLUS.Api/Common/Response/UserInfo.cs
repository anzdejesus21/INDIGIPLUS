using INDIGIPLUS.Api.Enum;

namespace INDIGIPLUS.Api.Common.Response
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
