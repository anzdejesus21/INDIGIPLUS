using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Client.DTOs.Quizs
{
    public class CreateQuizDto
    {
        #region Properties

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int LessonId { get; set; }

        #endregion Properties
    }
}