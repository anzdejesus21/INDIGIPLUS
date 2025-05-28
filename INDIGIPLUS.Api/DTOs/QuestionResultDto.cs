namespace INDIGIPLUS.Api.DTOs
{
    public class QuestionResultDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }
        public string? UserAnswer { get; set; }
        public string? CorrectAnswer { get; set; }
        public string Explanation { get; set; } = string.Empty;
    }
}
