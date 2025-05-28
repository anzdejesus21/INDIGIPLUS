using INDIGIPLUS.Api.Entities;

namespace INDIGIPLUS.Api.Repositories.Interfaces
{
    public interface ILessonRepository
    {
        #region Public Methods

        Task<List<Lesson>> GetLessonsByCourseAsync(int courseId);

        Task<Lesson?> GetLessonByIdAsync(int lessonId);

        Task AddLessonAsync(Lesson lesson);

        Task UpdateLessonAsync(Lesson lesson);

        Task<bool> DeleteLessonAsync(Lesson lesson);

        Task<UserProgress?> GetUserProgressAsync(int lessonId, int userId);

        Task AddUserProgressAsync(UserProgress progress);

        Task SaveChangesAsync();

        #endregion Public Methods
    }
}