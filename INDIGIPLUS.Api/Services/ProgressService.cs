using INDIGIPLUS.Api.Common.Enums;
using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Entities;
using INDIGIPLUS.Api.Repositories.Interfaces;
using INDIGIPLUS.Api.Services.Interfaces;

namespace INDIGIPLUS.Api.Services
{
    public class ProgressService(IProgressRepository progressRepository) : IProgressService
    {
        #region Public Methods

        public async Task<UserProgressDto> GetUserProgressAsync(int userId)
        {
            var userProgresses = await progressRepository.GetUserProgressesAsync(userId);
            var quizAttempts = await progressRepository.GetUserQuizAttemptsAsync(userId);

            var totalLessons = await progressRepository.GetTotalActiveLessonsCountAsync();
            var totalQuizzes = await progressRepository.GetTotalActiveQuizzesCountAsync();

            var completedLessons = userProgresses.Count(up => up.Status == ProgressStatus.Completed);

            var totalTimeSpent = userProgresses
                .Where(up => up.TimeSpent.HasValue)
                .Sum(up => up.TimeSpent.Value.TotalMinutes);

            var passedQuizzes = quizAttempts
                .Where(qa => qa.IsPassed)
                .Select(qa => qa.QuizId)
                .Distinct()
                .Count();

            var recentAchievements = (await progressRepository.GetUserAchievementsAsync(userId))
                .Take(5)
                .Select(ua => new AchievementDto
                {
                    Id = ua.Achievement.Id,
                    Name = ua.Achievement.Name,
                    Description = ua.Achievement.Description,
                    IconUrl = ua.Achievement.IconUrl,
                    EarnedAt = ua.EarnedAt
                })
                .ToList();

            return new UserProgressDto
            {
                UserId = userId,
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                TotalQuizzes = totalQuizzes,
                PassedQuizzes = passedQuizzes,
                OverallProgress = totalLessons > 0 ? (double) completedLessons / totalLessons * 100 : 0,
                TotalTimeSpentMinutes = (int) totalTimeSpent,
                RecentAchievements = recentAchievements
            };
        }

        public async Task<List<AchievementDto>> GetUserAchievementsAsync(int userId)
        {
            var userAchievements = await progressRepository.GetUserAchievementsAsync(userId);

            return userAchievements.Select(ua => new AchievementDto
            {
                Id = ua.Achievement.Id,
                Name = ua.Achievement.Name,
                Description = ua.Achievement.Description,
                IconUrl = ua.Achievement.IconUrl,
                EarnedAt = ua.EarnedAt
            }).ToList();
        }

        public async Task CheckAndAwardAchievementsAsync(int userId)
        {
            var userStats = await GetUserProgressAsync(userId);
            var existingAchievements = (await progressRepository.GetUserAchievementsAsync(userId))
                .Select(ua => ua.AchievementId)
                .ToList();

            var availableAchievements = await progressRepository.GetAvailableAchievementsAsync(existingAchievements);

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
                await progressRepository.AddUserAchievementsAsync(newAchievements);
                await progressRepository.SaveChangesAsync();
            }
        }

        #endregion Public Methods
    }
}