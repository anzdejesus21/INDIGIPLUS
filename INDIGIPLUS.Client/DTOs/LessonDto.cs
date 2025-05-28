using INDIGIPLUS.Client.Common.Enums;

namespace INDIGIPLUS.Client.DTOs
{
    public class LessonDto
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? CodeExample { get; set; }
        public int Order { get; set; }
        public int EstimatedMinutes { get; set; }
        public string Difficulty { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public bool HasQuiz { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Difficulty LessonDifficulty { get; set; }
        public UserProgressDto UserProgressDto { get; set; }
        public ProgressStatus UserProgress { get; set; }

        #endregion Properties
    }
}