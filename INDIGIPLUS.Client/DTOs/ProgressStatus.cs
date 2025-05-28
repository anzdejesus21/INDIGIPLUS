namespace INDIGIPLUS.Client.DTOs
{
    public class ProgressStatus
    {
        #region Properties

        public int UserId { get; set; }
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalQuizzes { get; set; }
        public int PassedQuizzes { get; set; }
        public double OverallProgress { get; set; }
        public int TotalTimeSpentMinutes { get; set; }
        public int CurrentStreak { get; set; }

        #endregion Properties
    }
}