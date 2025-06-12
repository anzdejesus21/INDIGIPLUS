using INDIGIPLUS.Client.Common.Enums;
using INDIGIPLUS.Client.DTOs.Answers;
using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Client.DTOs.Questions
{
    public class CreateQuestionDto
    {
        #region Properties
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
        public int QuizId { get; set; }
        public int OrderIndex { get; set; }
        public List<CreateAnswerDto> Answers { get; set; } = new();
        #endregion Properties
    }
}
