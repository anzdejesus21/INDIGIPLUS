using CppLearningPlatform.Models;
using INDIGIPLUS.Api.DTOs;

namespace INDIGIPLUS.Api.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetCoursesAsync(int userId);
        Task<CourseDto?> GetCourseByIdAsync(int courseId, int userId);
        Task<CourseDto> CreateCourseAsync(Course course);
        Task<CourseDto?> UpdateCourseAsync(int courseId, Course course);
        Task<bool> DeleteCourseAsync(int courseId);
    }
}
