using INDIGIPLUS.Api.Entities;

namespace INDIGIPLUS.Api.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        #region Public Methods

        Task<IEnumerable<Quiz>> GetAllAsync();

        Task<IEnumerable<Quiz>> GetByLessonIdAsync(int lessonId);

        Task<Quiz?> GetByIdAsync(int id);

        Task<Quiz?> GetByIdWithQuestionsAsync(int id);

        Task<Quiz> CreateAsync(Quiz quiz);

        Task<Quiz> UpdateAsync(Quiz quiz);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        #endregion Public Methods
    }
}