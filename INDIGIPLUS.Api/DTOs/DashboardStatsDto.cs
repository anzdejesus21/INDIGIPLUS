namespace INDIGIPLUS.Api.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalQuizzes { get; set; }
        public int PassedQuizzes { get; set; }
        public double OverallProgress { get; set; }
        public int CurrentStreak { get; set; }
        public List<RecentActivity> RecentActivity { get; set; } = new();
    }
}
