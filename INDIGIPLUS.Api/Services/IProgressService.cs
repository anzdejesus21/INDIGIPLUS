using INDIGIPLUS.Api.Common.Enums;
using INDIGIPLUS.Api.Data;
using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Entities;
using INDIGIPLUS.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INDIGIPLUS.Api.Services
{
    public class ProgressService : IProgressService
    {
        private readonly ApplicationDbContext _context;

        public ProgressService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProgressDto> GetUserProgressAsync(int userId)
        {
            // Get all user progress records
            var userProgresses = await _context.UserProgresses
                .Include(up => up.Lesson)
                .ThenInclude(l => l.Course)
                .Where(up => up.UserId == userId)
                .ToListAsync();

            // Get quiz attempts for statistics
            var quizAttempts = await _context.QuizAttempts
                .Include(qa => qa.Quiz)
                .ThenInclude(q => q.Lesson)
                .ThenInclude(l => l.Course)
                .Where(qa => qa.UserId == userId)
                .ToListAsync();

            // Calculate overall statistics
            var totalLessons = await _context.Lessons
                .Where(l => l.IsActive && l.Course.IsActive)
                .CountAsync();

            var completedLessons = userProgresses
                .Count(up => up.Status == ProgressStatus.Completed);

            var totalTimeSpent = userProgresses
                .Where(up => up.TimeSpent.HasValue)
                .Sum(up => up.TimeSpent.Value.TotalMinutes);

            var totalQuizzes = await _context.Quizzes
                .Where(q => q.IsActive && q.Lesson.IsActive && q.Lesson.Course.IsActive)
                .CountAsync();

            var passedQuizzes = quizAttempts
                .Where(qa => qa.IsPassed)
                .Select(qa => qa.QuizId)
                .Distinct()
                .Count();

            // Get recent achievements (last 5)
            var recentAchievements = await _context.UserAchievements
                .Include(ua => ua.Achievement)
                .Where(ua => ua.UserId == userId)
                .OrderByDescending(ua => ua.EarnedAt)
                .Take(5)
                .Select(ua => new AchievementDto
                {
                    Id = ua.Achievement.Id,
                    Name = ua.Achievement.Name,
                    Description = ua.Achievement.Description,
                    IconUrl = ua.Achievement.IconUrl,
                    EarnedAt = ua.EarnedAt
                })
                .ToListAsync();

            return new UserProgressDto
            {
                UserId = userId,
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                TotalQuizzes = totalQuizzes,
                PassedQuizzes = passedQuizzes,
                OverallProgress = totalLessons > 0 ? (double)completedLessons / totalLessons * 100 : 0,
                TotalTimeSpentMinutes = (int)totalTimeSpent,
                RecentAchievements = recentAchievements
            };
        }

        public async Task<List<AchievementDto>> GetUserAchievementsAsync(int userId)
        {
            var userAchievements = await _context.UserAchievements
                .Include(ua => ua.Achievement)
                .Where(ua => ua.UserId == userId)
                .Select(ua => new AchievementDto
                {
                    Id = ua.Achievement.Id,
                    Name = ua.Achievement.Name,
                    Description = ua.Achievement.Description,
                    IconUrl = ua.Achievement.IconUrl,
                    EarnedAt = ua.EarnedAt
                })
                .OrderByDescending(a => a.EarnedAt)
                .ToListAsync();

            return userAchievements;
        }

        public async Task CheckAndAwardAchievementsAsync(int userId)
        {
            var userStats = await GetUserProgressAsync(userId);
            var existingAchievements = await _context.UserAchievements
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.AchievementId)
                .ToListAsync();

            var availableAchievements = await _context.Achievements
                .Where(a => a.IsActive && !existingAchievements.Contains(a.Id))
                .ToListAsync();

            var newAchievements = new List<UserAchievement>();

            foreach (var achievement in availableAchievements)
            {
                bool shouldAward = false;

                switch (achievement.Name.ToLower())
                {
                    case "first lesson":
                        shouldAward = userStats.CompletedLessons >= 1;
                        break;
                    case "quiz master":
                        shouldAward = userStats.PassedQuizzes >= 5;
                        break;
                    case "dedicated learner":
                        shouldAward = userStats.CurrentStreak >= 7;
                        break;
                    case "course champion":
                        shouldAward = userStats.CompletedLessons >= 10;
                        break;
                }

                if (shouldAward)
                {
                    newAchievements.Add(new UserAchievement
                    {
                        UserId = userId,
                        AchievementId = achievement.Id,
                        EarnedAt = DateTime.UtcNow
                    });
                }
            }

            if (newAchievements.Any())
            {
                _context.UserAchievements.AddRange(newAchievements);
                await _context.SaveChangesAsync();
            }
        }
    }
}