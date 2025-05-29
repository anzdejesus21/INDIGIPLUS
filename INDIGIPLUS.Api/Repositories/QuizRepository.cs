using INDIGIPLUS.Api.Data;
using INDIGIPLUS.Api.Entities;
using INDIGIPLUS.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INDIGIPLUS.Api.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion Fields

        #region Public Constructors

        public QuizRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<IEnumerable<Quiz>> GetAllAsync()
        {
            return await _context.Quizzes
                .Include(q => q.Lesson)
                .Where(q => q.IsActive)
                .OrderBy(q => q.Lesson.OrderIndex)
                .ThenBy(q => q.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quiz>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.Quizzes
                .Where(q => q.LessonId == lessonId && q.IsActive)
                .OrderBy(q => q.Title)
                .ToListAsync();
        }

        public async Task<Quiz?> GetByIdAsync(int id)
        {
            return await _context.Quizzes
                .Include(q => q.Lesson)
                .FirstOrDefaultAsync(q => q.Id == id && q.IsActive);
        }

        public async Task<Quiz?> GetByIdWithQuestionsAsync(int id)
        {
            return await _context.Quizzes
                .Include(q => q.Lesson)
                .Include(q => q.Questions.Where(qu => qu.OrderIndex >= 0))
                    .ThenInclude(qu => qu.Answers)
                .FirstOrDefaultAsync(q => q.Id == id && q.IsActive);
        }

        public async Task<Quiz> CreateAsync(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<Quiz> UpdateAsync(Quiz quiz)
        {
            quiz.UpdatedAt = DateTime.UtcNow;
            _context.Entry(quiz).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
                return false;

            quiz.IsActive = false;
            quiz.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Quizzes.AnyAsync(q => q.Id == id && q.IsActive);
        }

        #endregion Public Methods
    }
}