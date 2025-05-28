using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Entities;

namespace INDIGIPLUS.Api.Services.Interfaces
{
    public interface ILessonService
    {
        Task<List<LessonDto>> GetLessonsByCourseAsync(int courseId, int userId);
        Task<LessonDto?> GetLessonByIdAsync(int lessonId, int userId);
        Task<LessonDto> CreateLessonAsync(Lesson lesson);
        Task<LessonDto?> UpdateLessonAsync(int lessonId, Lesson lesson);
        Task<bool> DeleteLessonAsync(int lessonId);
        Task<bool> MarkLessonAsStartedAsync(int lessonId, int userId);
        Task<bool> MarkLessonAsCompletedAsync(int lessonId, int userId);
    }
}
