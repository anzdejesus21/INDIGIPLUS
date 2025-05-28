namespace INDIGIPLUS.Api.Entities
{
    public class UserAchievement
    {
        public int Id { get; set; }
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int AchievementId { get; set; }
        public virtual Achievement Achievement { get; set; } = null!;
    }
}
