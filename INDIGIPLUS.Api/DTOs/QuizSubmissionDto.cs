namespace INDIGIPLUS.Api.DTOs
{
    public class QuizSubmissionDto
    {
        public int QuizId { get; set; }
        public List<UserAnswerSubmissionDto> Answers { get; set; } = new();
    }
}
