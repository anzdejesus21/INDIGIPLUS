using CppLearningPlatform.Models;
using INDIGIPLUS.Api.Common.Enums;
using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Repositories.Interfaces;
using INDIGIPLUS.Api.Services.Interfaces;

namespace INDIGIPLUS.Api.Services
{
    public class CourseService(ICourseRepository courseRepository) : ICourseService
    {
        #region Public Methods

        public async Task<List<CourseDto>> GetCoursesAsync(int userId)
        {
            var courses = await courseRepository.GetActiveCoursesAsync();

            return courses.Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                Order = c.Order,
                LessonCount = c.Lessons.Count(l => l.IsActive),
                CompletedLessons = c.Lessons
                    .SelectMany(l => l.UserProgresses)
                    .Count(up => up.UserId == userId && up.Status == ProgressStatus.Completed),
                ProgressPercentage = c.Lessons.Count(l => l.IsActive) > 0
                    ? (double) c.Lessons
                        .SelectMany(l => l.UserProgresses)
                        .Count(up => up.UserId == userId && up.Status == ProgressStatus.Completed) /
                      c.Lessons.Count(l => l.IsActive) * 100
                    : 0
            }).ToList();
        }

        public async Task<CourseDto?> GetCourseByIdAsync(int courseId, int userId)
        {
            var c = await courseRepository.GetActiveCourseByIdAsync(courseId);
            if (c == null)
                return null;

            return new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                Order = c.Order,
                LessonCount = c.Lessons.Count(l => l.IsActive),
                CompletedLessons = c.Lessons
                    .SelectMany(l => l.UserProgresses)
                    .Count(up => up.UserId == userId && up.Status == ProgressStatus.Completed),
                ProgressPercentage = c.Lessons.Count(l => l.IsActive) > 0
                    ? (double) c.Lessons
                        .SelectMany(l => l.UserProgresses)
                        .Count(up => up.UserId == userId && up.Status == ProgressStatus.Completed) /
                      c.Lessons.Count(l => l.IsActive) * 100
                    : 0
            };
        }

        public async Task<CourseDto> CreateCourseAsync(Course course)
        {
            await courseRepository.AddCourseAsync(course);
            await courseRepository.SaveChangesAsync();

            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                ImageUrl = course.ImageUrl,
                Order = course.Order,
                LessonCount = 0,
                CompletedLessons = 0,
                ProgressPercentage = 0
            };
        }

        public async Task<CourseDto?> UpdateCourseAsync(int courseId, Course course)
        {
            var existingCourse = await courseRepository.FindCourseByIdAsync(courseId);
            if (existingCourse == null)
                return null;

            existingCourse.Title = course.Title;
            existingCourse.Description = course.Description;
            existingCourse.ImageUrl = course.ImageUrl;
            existingCourse.Order = course.Order;
            existingCourse.IsActive = course.IsActive;
            existingCourse.UpdatedAt = DateTime.UtcNow;

            await courseRepository.SaveChangesAsync();

            return new CourseDto
            {
                Id = existingCourse.Id,
                Title = existingCourse.Title,
                Description = existingCourse.Description,
                ImageUrl = existingCourse.ImageUrl,
                Order = existingCourse.Order
            };
        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await courseRepository.FindCourseByIdAsync(courseId);
            if (course == null)
                return false;

            course.IsActive = false;
            course.UpdatedAt = DateTime.UtcNow;
            await courseRepository.SaveChangesAsync();

            return true;
        }

        #endregion Public Methods
    }
}