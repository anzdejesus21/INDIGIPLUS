using INDIGIPLUS.Api.Common.Enums;

namespace INDIGIPLUS.Api.DTOs
{
    public class UpdateQuestionDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public string? CodeSnippet { get; set; }
        public QuestionType Type { get; set; }
        public int Points { get; set; } = 1;
        public int Order { get; set; }
        public string Explanation { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public List<CreateAnswerOptionDto>? AnswerOptions { get; set; }
    }
}