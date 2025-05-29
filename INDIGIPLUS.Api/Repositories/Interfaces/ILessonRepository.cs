using INDIGIPLUS.Api.Entities;

namespace INDIGIPLUS.Api.Repositories.Interfaces
{
    public interface ILessonRepository
    {
        #region Public Methods

        Task<IEnumerable<Lesson>> GetAllAsync();

        Task<Lesson?> GetByIdAsync(int id);

        Task<Lesson?> GetByIdWithQuizzesAsync(int id);

        Task<Lesson> CreateAsync(Lesson lesson);

        Task<Lesson> UpdateAsync(Lesson lesson);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        #endregion Public Methods
    }
}