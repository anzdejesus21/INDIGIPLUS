using INDIGIPLUS.Api.Common.Enums;

namespace INDIGIPLUS.Api.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string? CodeSnippet { get; set; }
        public QuestionType Type { get; set; }
        public int Points { get; set; }
        public int Order { get; set; }
        public List<AnswerOptionDto> AnswerOptions { get; set; } = new();
    }
}
