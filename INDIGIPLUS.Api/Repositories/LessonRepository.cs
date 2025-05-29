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

        public async Task<IEnumerable<Lesson>> GetAllAsync()
        {
            return await _context.Lessons
                .Where(l => l.IsActive)
                .OrderBy(l => l.OrderIndex)
                .ToListAsync();
        }

        public async Task<Lesson?> GetByIdAsync(int id)
        {
            return await _context.Lessons
                .FirstOrDefaultAsync(l => l.Id == id && l.IsActive);
        }

        public async Task<Lesson?> GetByIdWithQuizzesAsync(int id)
        {
            return await _context.Lessons
                .Include(l => l.Quizzes.Where(q => q.IsActive))
                .FirstOrDefaultAsync(l => l.Id == id && l.IsActive);
        }

        public async Task<Lesson> CreateAsync(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task<Lesson> UpdateAsync(Lesson lesson)
        {
            lesson.UpdatedAt = DateTime.UtcNow;
            _context.Entry(lesson).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
                return false;

            lesson.IsActive = false;
            lesson.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Lessons.AnyAsync(l => l.Id == id && l.IsActive);
        }

        #endregion Public Methods
    }
}