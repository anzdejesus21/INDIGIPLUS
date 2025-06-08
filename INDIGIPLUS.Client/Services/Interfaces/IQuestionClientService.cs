using INDIGIPLUS.Client.DTOs.Questions;

namespace INDIGIPLUS.Client.Services.Interfaces
{
    public interface IQuestionClientService
    {
        #region Public Methods
        Task<List<QuestionDto>> GetAllQuestionsAsync();
        Task<List<QuestionDto>> GetQuestionsByQuizAsync(int quizId);
        Task<QuestionDto?> GetQuestionByIdAsync(int id);
        Task<QuestionDto?> CreateQuestionAsync(CreateQuestionDto dto);
        Task<QuestionDto?> UpdateQuestionAsync(int id, UpdateQuestionDto dto);
        Task<bool> DeleteQuestionAsync(int id);
        #endregion Public Methods
    }
}