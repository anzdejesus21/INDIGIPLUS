using INDIGIPLUS.Api.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Api.Entities
{
    public class User
    {
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

        public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
        public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();

    }
}
