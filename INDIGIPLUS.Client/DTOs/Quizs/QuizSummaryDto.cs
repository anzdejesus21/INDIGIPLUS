namespace INDIGIPLUS.Client.DTOs.Quizs
{
    public class QuizSummaryDto
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        #endregion Properties
    }
}