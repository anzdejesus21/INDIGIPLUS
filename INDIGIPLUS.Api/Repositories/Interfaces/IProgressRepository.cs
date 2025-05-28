using INDIGIPLUS.Api.Entities;

namespace INDIGIPLUS.Api.Repositories.Interfaces
{
    public interface IProgressRepository
    {
        #region Public Methods

        Task<List<UserProgress>> GetUserProgressesAsync(int userId);

        Task<List<QuizAttempt>> GetUserQuizAttemptsAsync(int userId);

        Task<int> GetTotalActiveLessonsCountAsync();

        Task<int> GetTotalActiveQuizzesCountAsync();

        Task<List<UserAchievement>> GetUserAchievementsAsync(int userId);

        Task<List<Achievement>> GetAvailableAchievementsAsync(List<int> excludeIds);

        Task AddUserAchievementsAsync(List<UserAchievement> achievements);

        Task SaveChangesAsync();

        #endregion Public Methods
    }
}