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

        public async Task<List<Quiz>> GetQuizzesByLessonAsync(int lessonId)
        {
            return await _context.Quizzes
                .Include(q => q.Questions)
                .Include(q => q.QuizAttempts)
                .Where(q => q.LessonId == lessonId && q.IsActive)
                .ToListAsync();
        }

        public async Task<Quiz?> GetQuizByIdAsync(int quizId)
        {
            return await _context.Quizzes
                .Include(q => q.Questions)
                .Include(q => q.QuizAttempts)
                .FirstOrDefaultAsync(q => q.Id == quizId && q.IsActive);
        }

        public async Task<List<Question>> GetQuizQuestionsAsync(int quizId)
        {
            return await _context.Questions
                .Include(q => q.AnswerOptions)
                .Where(q => q.QuizId == quizId && q.IsActive)
                .OrderBy(q => q.Order)
                .ToListAsync();
        }

        public async Task AddQuizAsync(Quiz quiz)
        {
            await _context.Quizzes.AddAsync(quiz);
        }

        public async Task UpdateQuizAsync(Quiz quiz)
        {
            _context.Quizzes.Update(quiz);
            await Task.CompletedTask;
        }

        public async Task<Quiz?> FindQuizByIdAsync(int quizId)
        {
            return await _context.Quizzes.FindAsync(quizId);
        }

        public async Task AddUserAnswerAsync(UserAnswer userAnswer)
        {
            await _context.UserAnswers.AddAsync(userAnswer);
        }

        public async Task<bool> SoftDeleteQuizAsync(int quizId)
        {
            var quiz = await FindQuizByIdAsync(quizId);
            if (quiz == null || !quiz.IsActive)
                return false;

            quiz.IsActive = false;
            return true;
        }

        public async Task AddQuizAttemptAsync(QuizAttempt attempt)
        {
            await _context.QuizAttempts.AddAsync(attempt);
        }

        public async Task<List<QuizAttempt>> GetUserQuizAttemptsAsync(int userId, int quizId)
        {
            return await _context.QuizAttempts
                .Include(qa => qa.UserAnswers)
                    .ThenInclude(ua => ua.Question)
                        .ThenInclude(q => q.AnswerOptions)
                .Where(qa => qa.UserId == userId && qa.QuizId == quizId)
                .OrderByDescending(qa => qa.StartedAt)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion Public Methods
    }
}