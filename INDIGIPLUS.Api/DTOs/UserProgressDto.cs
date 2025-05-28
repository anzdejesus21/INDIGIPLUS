namespace INDIGIPLUS.Api.DTOs
{
    public class UserProgressDto
    {
        public int UserId { get; set; }
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalQuizzes { get; set; }
        public int PassedQuizzes { get; set; }
        public double OverallProgress { get; set; }
        public int TotalTimeSpentMinutes { get; set; }
        public int CurrentStreak { get; set; }
        public List<AchievementDto> RecentAchievements { get; set; } = new();
    }
}
