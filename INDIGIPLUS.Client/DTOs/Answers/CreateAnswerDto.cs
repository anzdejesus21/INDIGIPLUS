using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Client.DTOs.Answers
{
    public class CreateAnswerDto
    {
        [Required, MaxLength(500)]
        #region Properties
        public string AnswerText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public string? Explanation { get; set; }
        public int OrderIndex { get; set; }

        #endregion Properties
    }
}