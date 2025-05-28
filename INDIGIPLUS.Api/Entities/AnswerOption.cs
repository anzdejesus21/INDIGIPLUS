namespace INDIGIPLUS.Api.Entities
{
    public class AnswerOption
    {
        public int Id { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int Order { get; set; }

        // Foreign key
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; } = null!;
    }
}
