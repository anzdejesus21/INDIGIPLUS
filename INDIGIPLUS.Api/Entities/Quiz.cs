using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Api.Entities
{
    public class Quiz
    {
        #region Properties

        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public int LessonId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Lesson Lesson { get; set; } = null!;

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

        #endregion Properties
    }
}