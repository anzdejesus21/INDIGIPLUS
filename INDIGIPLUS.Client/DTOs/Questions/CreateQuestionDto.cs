using INDIGIPLUS.Client.DTOs.Answers;
using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Client.DTOs.Questions
{
    public class CreateQuestionDto
    {
        #region Properties
        [Required, MaxLength(500)]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty; // "MultipleChoice", "TrueFalse", "Identification"

        [Required]
        public int QuizId { get; set; }

        public int OrderIndex { get; set; }

        public List<CreateAnswerDto> Answers { get; set; } = new();
        #endregion Properties
    }
}