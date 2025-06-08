namespace INDIGIPLUS.Client.Models
{
    public class QuestionFormModel
    {
        #region Properties
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string Type { get; set; } = "MultipleChoice";
        public int QuizId { get; set; }
        public int OrderIndex { get; set; }
        public List<AnswerFormModel> Answers { get; set; } = new();
        #endregion Properties
    }

    public class AnswerFormModel
    {
        public int Id { get; set; }
        public string AnswerText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OrderIndex { get; set; }
    }
}