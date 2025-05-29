using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.DTOs.Quizs;

namespace INDIGIPLUS.Api.Services.Interfaces
{
    public interface IQuizService
    {
        #region Public Methods

        Task<IEnumerable<QuizDto>> GetAllQuizzesAsync();

        Task<IEnumerable<QuizDto>> GetQuizzesByLessonIdAsync(int lessonId);

        Task<QuizDto?> GetQuizByIdAsync(int id);

        Task<QuizWithQuestionsDto?> GetQuizWithQuestionsAsync(int id);

        Task<QuizDto> CreateQuizAsync(CreateQuizDto dto);

        Task<QuizDto?> UpdateQuizAsync(int id, UpdateQuizDto dto);

        Task<bool> DeleteQuizAsync(int id);

        #endregion Public Methods
    }
}