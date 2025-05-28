using INDIGIPLUS.Api.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Api.Entities
{
    public class Achievement
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public AchievementType Type { get; set; }
        public string Criteria { get; set; } = string.Empty; // JSON criteria
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
    }
}
