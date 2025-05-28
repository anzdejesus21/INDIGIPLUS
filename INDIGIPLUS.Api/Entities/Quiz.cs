using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Api.Entities
{
    public class Quiz
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public int TimeLimit { get; set; } = 30; // minutes
        public int PassingScore { get; set; } = 70; // percentage
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key
        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}
