using INDIGIPLUS.Api.Common.Enums;
using INDIGIPLUS.Api.Data;
using INDIGIPLUS.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace INDIGIPLUS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardStatsDto>> GetDashboardStats()
        {
            var userId = GetUserId();
            if (userId == 0)
                return Unauthorized();

            var stats = new DashboardStatsDto();

            // Get basic progress stats
            var totalLessons = await _context.Lessons.CountAsync(l => l.IsActive);
            var completedLessons = await _context.UserProgresses
                .CountAsync(up => up.UserId == userId && up.Status == ProgressStatus.Completed);

            var totalQuizzes = await _context.Quizzes.CountAsync(q => q.IsActive);
            var passedQuizzes = await _context.QuizAttempts
                .CountAsync(qa => qa.UserId == userId && qa.IsPassed);

            stats.TotalLessons = totalLessons;
            stats.CompletedLessons = completedLessons;
            stats.TotalQuizzes = totalQuizzes;
            stats.PassedQuizzes = passedQuizzes;
            stats.OverallProgress = totalLessons > 0 ? (double) completedLessons / totalLessons * 100 : 0;

            // Get recent activity
            var recentActivity = await _context.UserProgresses
                .Where(up => up.UserId == userId && up.LastAccessedAt >= DateTime.UtcNow.AddDays(-7))
                .OrderByDescending(up => up.LastAccessedAt)
                .Take(5)
                .Select(up => new RecentActivity
                {
                    LessonTitle = up.Lesson.Title,
                    ActivityType = up.Status == ProgressStatus.Completed ? "Completed" : "Studied",
                    Date = up.LastAccessedAt
                })
                .ToListAsync();

            stats.RecentActivity = recentActivity;

            // Calculate current streak
            var today = DateTime.UtcNow.Date;
            var streak = 0;
            var checkDate = today;

            while (true)
            {
                var hasActivity = await _context.UserProgresses
                    .AnyAsync(up => up.UserId == userId &&
                                   up.LastAccessedAt.Date == checkDate);

                if (hasActivity)
                {
                    streak++;
                    checkDate = checkDate.AddDays(-1);
                }
                else
                {
                    break;
                }
            }

            stats.CurrentStreak = streak;

            return Ok(stats);
        }

        [HttpGet("progress-chart")]
        public async Task<ActionResult<List<ProgressChartData>>> GetProgressChart()
        {
            var userId = GetUserId();
            if (userId == 0)
                return Unauthorized();

            var last30Days = DateTime.UtcNow.AddDays(-30).Date;

            var progressData = await _context.UserProgresses
                .Where(up => up.UserId == userId && up.CompletedAt >= last30Days)
                .GroupBy(up => up.CompletedAt!.Value.Date)
                .Select(g => new ProgressChartData
                {
                    Date = g.Key,
                    LessonsCompleted = g.Count()
                })
                .OrderBy(p => p.Date)
                .ToListAsync();

            return Ok(progressData);
        }
    }
}
