using INDIGIPLUS.Api.Common.Enums;

namespace INDIGIPLUS.Api.DTOs
{
    public class LessonDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CodeExample { get; set; } = string.Empty;
        public int Order { get; set; }
        public int EstimatedMinutes { get; set; }
        public LessonDifficulty Difficulty { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public bool HasQuiz { get; set; }
        public ProgressStatus UserProgress { get; set; }
    }
}
