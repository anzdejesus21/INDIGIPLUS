namespace INDIGIPLUS.Client.DTOs.Answers
{
    public class AnswerDto
    {
       #region Properties
        public int Id { get; set; }
        public string AnswerText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public string? Explanation { get; set; }
        public int QuestionId { get; set; }
        public int OrderIndex { get; set; }
        #endregion Properties
    }
}