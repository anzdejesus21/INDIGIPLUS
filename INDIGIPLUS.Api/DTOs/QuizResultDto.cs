namespace INDIGIPLUS.Api.DTOs
{
    public class QuizResultDto
    {
        public int AttemptId { get; set; }
        public int Score { get; set; }
        public int TotalPoints { get; set; }
        public double Percentage { get; set; }
        public bool IsPassed { get; set; }
        public TimeSpan Duration { get; set; }
        public List<QuestionResultDto> QuestionResults { get; set; } = new();
    }
}
