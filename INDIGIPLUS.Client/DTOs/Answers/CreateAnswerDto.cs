using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Client.DTOs.Answers
{
    public class CreateAnswerDto
    {
        [Required, MaxLength(500)]
        public string AnswerText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        public int OrderIndex { get; set; }
    }
}