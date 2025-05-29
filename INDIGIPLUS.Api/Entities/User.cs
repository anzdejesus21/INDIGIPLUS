using INDIGIPLUS.Api.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Api.Entities
{
    public class User
    {
        #region Properties

        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        #endregion Properties
    }
}