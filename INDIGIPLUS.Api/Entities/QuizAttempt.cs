namespace INDIGIPLUS.Api.Entities
{
    public class QuizAttempt
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int TotalPoints { get; set; }
        public double Percentage { get; set; }
        public bool IsPassed { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public TimeSpan? Duration { get; set; }

        // Foreign keys
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}
