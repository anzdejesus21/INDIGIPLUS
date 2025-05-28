using INDIGIPLUS.Client.DTOs;

namespace INDIGIPLUS.Client.Services.Interfaces
{
    public interface ILessonClientService
    {
        #region Public Methods

        // Lesson methods
        Task<List<LessonDto>> GetLessonsByCourseAsync(int courseId);

        Task<LessonDto?> GetLessonByIdAsync(int lessonId);

        Task<LessonDto> CreateLessonAsync(LessonDto lesson);

        Task<LessonDto?> UpdateLessonAsync(int lessonId, LessonDto lesson);

        Task<bool> DeleteLessonAsync(int lessonId);

        Task<bool> StartLessonAsync(int lessonId);

        Task<bool> CompleteLessonAsync(int lessonId);

        // Course methods
        Task<List<CourseDto>> GetCoursesAsync();

        Task<CourseDto?> GetCourseByIdAsync(int courseId);

        Task<CourseDto> CreateCourseAsync(CourseDto course);

        Task<CourseDto?> UpdateCourseAsync(int courseId, CourseDto course);

        Task<bool> DeleteCourseAsync(int courseId);

        #endregion Public Methods
    }
}