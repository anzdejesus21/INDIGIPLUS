using CppLearningPlatform.Models;
using INDIGIPLUS.Api.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace INDIGIPLUS.Api.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; // HTML/Markdown content
        public string CodeExample { get; set; } = string.Empty; // C++ code examples
        public int Order { get; set; }
        public int EstimatedMinutes { get; set; } = 15;
        public LessonDifficulty Difficulty { get; set; } = LessonDifficulty.Beginner;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key
        [JsonIgnore]
        public int CourseId { get; set; }

        [JsonIgnore]
        public virtual Course Course { get; set; } = null!;

        // Navigation properties
        [JsonIgnore]
        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

        [JsonIgnore]
        public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
    }
}
