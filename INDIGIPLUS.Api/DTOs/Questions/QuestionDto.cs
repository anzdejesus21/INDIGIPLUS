using INDIGIPLUS.Api.DTOs.Answers;

namespace INDIGIPLUS.Api.DTOs.Questions
{
    public class QuestionDto
    {
        #region Properties

        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public int OrderIndex { get; set; }
        public List<AnswerDto> Answers { get; set; } = new();

        #endregion Properties
    }
}