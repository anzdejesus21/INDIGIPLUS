using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Client.DTOs.Lessons
{
    public class UpdateLessonDto
    {
        #region Properties

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public int OrderIndex { get; set; }

        #endregion Properties
    }
}