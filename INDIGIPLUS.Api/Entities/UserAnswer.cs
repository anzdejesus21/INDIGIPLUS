namespace INDIGIPLUS.Api.Entities
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public string? AnswerText { get; set; } // For text/code answers
        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public int QuizAttemptId { get; set; }
        public virtual QuizAttempt QuizAttempt { get; set; } = null!;

        public int QuestionId { get; set; }
        public virtual Question Question { get; set; } = null!;

        public int? SelectedAnswerOptionId { get; set; } // For multiple choice
        public virtual AnswerOption? SelectedAnswerOption { get; set; }
    }
}
