using INDIGIPLUS.Api.Entities;

namespace INDIGIPLUS.Api.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        #region Public Methods

        Task<IEnumerable<Question>> GetByQuizIdAsync(int quizId);

        Task<Question?> GetByIdAsync(int id);

        Task<Question?> GetByIdWithAnswersAsync(int id);

        Task<Question> CreateAsync(Question question);

        Task<Question> UpdateAsync(Question question);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        #endregion Public Methods
    }
}