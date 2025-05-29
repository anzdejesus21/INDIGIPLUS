using INDIGIPLUS.Client.DTOs.Quizs;
using System.Buffers.Text;
using System.Net.Http;

namespace INDIGIPLUS.Client.Services.Interfaces
{
    public interface IQuizClientService
    {
        #region Public Methods

        Task<List<QuizDto>> GetAllQuizzesAsync();

        Task<List<QuizDto>> GetQuizzesByLessonAsync(int lessonId);

        Task<QuizDto?> GetQuizByIdAsync(int id);

        Task<QuizWithQuestionsDto?> GetQuizWithQuestionsAsync(int id);

        Task<QuizDto?> CreateQuizAsync(CreateQuizDto dto);

        Task<QuizDto?> UpdateQuizAsync(int id, UpdateQuizDto dto);

        Task<bool> DeleteQuizAsync(int id);

        #endregion Public Methods
    }
}