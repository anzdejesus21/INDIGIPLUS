namespace INDIGIPLUS.Api.DTOs
{
    public class UserAnswerSubmissionDto
    {
        public int QuestionId { get; set; }
        public int? SelectedAnswerOptionId { get; set; }
        public string? AnswerText { get; set; }
    }
}
