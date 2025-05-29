using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Api.DTOs.Answers
{
    public class CreateAnswerDto
    {
        #region Properties

        [Required]
        public string AnswerText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        #endregion Properties
    }
}