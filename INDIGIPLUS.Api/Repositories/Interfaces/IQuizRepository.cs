using INDIGIPLUS.Api.Entities;

namespace INDIGIPLUS.Api.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        #region Public Methods

        Task<List<Quiz>> GetQuizzesByLessonAsync(int lessonId);

        Task<Quiz?> GetQuizByIdAsync(int quizId);

        Task<List<Question>> GetQuizQuestionsAsync(int quizId);

        Task AddUserAnswerAsync(UserAnswer userAnswer);

        Task AddQuizAsync(Quiz quiz);

        Task UpdateQuizAsync(Quiz quiz);

        Task<Quiz?> FindQuizByIdAsync(int quizId);

        Task<bool> SoftDeleteQuizAsync(int quizId);

        Task AddQuizAttemptAsync(QuizAttempt attempt);

        Task<List<QuizAttempt>> GetUserQuizAttemptsAsync(int userId, int quizId);

        Task SaveChangesAsync();

        #endregion Public Methods
    }
}