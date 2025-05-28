using INDIGIPLUS.Api.Data;
using INDIGIPLUS.Api.Entities;
using INDIGIPLUS.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INDIGIPLUS.Api.Repositories
{
    public class ProgressRepository : IProgressRepository
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion Fields

        #region Public Constructors

        public ProgressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<List<UserProgress>> GetUserProgressesAsync(int userId)
        {
            return await _context.UserProgresses
                .Include(up => up.Lesson)
                .ThenInclude(l => l.Course)
                .Where(up => up.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<QuizAttempt>> GetUserQuizAttemptsAsync(int userId)
        {
            return await _context.QuizAttempts
                .Include(qa => qa.Quiz)
                .ThenInclude(q => q.Lesson)
                .ThenInclude(l => l.Course)
                .Where(qa => qa.UserId == userId)
                .ToListAsync();
        }

        public async Task<int> GetTotalActiveLessonsCountAsync()
        {
            return await _context.Lessons
                .Where(l => l.IsActive && l.Course.IsActive)
                .CountAsync();
        }

        public async Task<int> GetTotalActiveQuizzesCountAsync()
        {
            return await _context.Quizzes
                .Where(q => q.IsActive && q.Lesson.IsActive && q.Lesson.Course.IsActive)
                .CountAsync();
        }

        public async Task<List<UserAchievement>> GetUserAchievementsAsync(int userId)
        {
            return await _context.UserAchievements
                .Include(ua => ua.Achievement)
                .Where(ua => ua.UserId == userId)
                .OrderByDescending(ua => ua.EarnedAt)
                .ToListAsync();
        }

        public async Task<List<Achievement>> GetAvailableAchievementsAsync(List<int> excludeIds)
        {
            return await _context.Achievements
                .Where(a => a.IsActive && !excludeIds.Contains(a.Id))
                .ToListAsync();
        }

        public async Task AddUserAchievementsAsync(List<UserAchievement> achievements)
        {
            await _context.UserAchievements.AddRangeAsync(achievements);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion Public Methods
    }
}