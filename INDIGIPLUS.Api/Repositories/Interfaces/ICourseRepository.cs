using CppLearningPlatform.Models;

namespace INDIGIPLUS.Api.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        #region Public Methods

        Task<List<Course>> GetActiveCoursesAsync();

        Task<Course?> GetActiveCourseByIdAsync(int courseId);

        Task AddCourseAsync(Course course);

        Task<Course?> FindCourseByIdAsync(int courseId);

        Task SaveChangesAsync();

        #endregion Public Methods
    }
}