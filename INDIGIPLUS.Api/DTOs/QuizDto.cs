namespace INDIGIPLUS.Api.DTOs
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TimeLimit { get; set; }
        public int PassingScore { get; set; }
        public int LessonId { get; set; }
        public int QuestionCount { get; set; }
        public int? BestScore { get; set; }
        public int AttemptCount { get; set; }
        public bool IsPassed { get; set; }
    }
}
