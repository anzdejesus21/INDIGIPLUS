using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Entities;

namespace INDIGIPLUS.Api.Services.Interfaces
{
    public interface IQuizService
    {
        Task<List<QuizDto>> GetQuizzesByLessonAsync(int lessonId, int userId);
        Task<QuizDto?> GetQuizByIdAsync(int quizId, int userId);
        Task<List<QuestionDto>> GetQuizQuestionsAsync(int quizId);
        Task<QuizResultDto> SubmitQuizAsync(int userId, QuizSubmissionDto submission);
        Task<List<QuizResultDto>> GetUserQuizAttemptsAsync(int userId, int quizId);
        Task<QuizDto> CreateQuizAsync(Quiz quiz);
        Task<QuizDto?> UpdateQuizAsync(int quizId, Quiz quiz);
        Task<bool> DeleteQuizAsync(int quizId);
    }
}
