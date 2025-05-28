using INDIGIPLUS.Api.Common.Enums;

namespace INDIGIPLUS.Api.Entities
{
    public class UserProgress
    {
        public int Id { get; set; }
        public ProgressStatus Status { get; set; } = ProgressStatus.NotStarted;
        public int CompletionPercentage { get; set; } = 0;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TimeSpan? TimeSpent { get; set; }
        public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; } = null!;
    }
}
