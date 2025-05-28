using INDIGIPLUS.Api.Entities;
using System.ComponentModel.DataAnnotations;

namespace CppLearningPlatform.Models
{
    public class Course
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Order { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}