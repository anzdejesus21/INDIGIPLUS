using INDIGIPLUS.Api.Data;
using INDIGIPLUS.Api.Entities;
using INDIGIPLUS.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INDIGIPLUS.Api.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion Fields

        #region Public Constructors

        public LessonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<List<Lesson>> GetLessonsByCourseAsync(int courseId)
        {
            return await _context.Lessons
                .Include(l => l.Course)
                .Include(l => l.Quizzes)
                .Include(l => l.UserProgresses)
                .Where(l => l.CourseId == courseId && l.IsActive)
                .OrderBy(l => l.Order)
                .ToListAsync();
        }

        public async Task<Lesson?> GetLessonByIdAsync(int lessonId)
        {
            return await _context.Lessons
                .Include(l => l.Course)
                .Include(l => l.Quizzes)
                .Include(l => l.UserProgresses)
                .FirstOrDefaultAsync(l => l.Id == lessonId && l.IsActive);
        }

        public async Task AddLessonAsync(Lesson lesson)
        {
            await _context.Lessons.AddAsync(lesson);
        }

        public async Task UpdateLessonAsync(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
        }

        public async Task<bool> DeleteLessonAsync(Lesson lesson)
        {
            lesson.IsActive = false;
            lesson.UpdatedAt = DateTime.UtcNow;
            _context.Lessons.Update(lesson);
            return true;
        }

        public async Task<UserProgress?> GetUserProgressAsync(int lessonId, int userId)
        {
            return await _context.UserProgresses
                .FirstOrDefaultAsync(up => up.LessonId == lessonId && up.UserId == userId);
        }

        public async Task AddUserProgressAsync(UserProgress progress)
        {
            await _context.UserProgresses.AddAsync(progress);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion Public Methods
    }
}