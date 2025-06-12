namespace INDIGIPLUS.Client.Models
{
    public class AnswerFormModel
    {
        public int Id { get; set; }
        public string AnswerText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OrderIndex { get; set; }
    }
}
