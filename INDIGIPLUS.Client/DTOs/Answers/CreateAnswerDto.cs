using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Client.DTOs.Answers
{
    public class CreateAnswerDto
    {
        #region Properties
        [Required, MaxLength(300)]
        public string AnswerText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        public int OrderIndex { get; set; }
        #endregion Properties
    }
}
