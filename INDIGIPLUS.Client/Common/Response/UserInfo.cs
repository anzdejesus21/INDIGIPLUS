using INDIGIPLUS.Client.Common.Enums;

namespace INDIGIPLUS.Client.Common.Response
{
    public class UserInfo
    {
        #region Properties

        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        #endregion Properties
    }
}