using INDIGIPLUS.Api.Common.Enums;

namespace INDIGIPLUS.Api.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string? CodeSnippet { get; set; } // Optional C++ code snippet
        public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
        public int Points { get; set; } = 1;
        public int Order { get; set; }
        public string Explanation { get; set; } = string.Empty; // Explanation for the answer
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key
        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
        public virtual ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}
