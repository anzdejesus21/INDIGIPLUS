using CppLearningPlatform.Models;
using INDIGIPLUS.Api.Common.Enums;
using INDIGIPLUS.Api.Data;
using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INDIGIPLUS.Api.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseDto>> GetCoursesAsync(int userId)
        {
            var courses = await _context.Courses
                .Where(c => c.IsActive)
                .OrderBy(c => c.Order)
                .Select(c => new CourseDto
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
                        ? (double)c.Lessons
                            .SelectMany(l => l.UserProgresses)
                            .Count(up => up.UserId == userId && up.Status == ProgressStatus.Completed) /
                          c.Lessons.Count(l => l.IsActive) * 100
                        : 0
                })
                .ToListAsync();

            return courses;
        }

        public async Task<CourseDto?> GetCourseByIdAsync(int courseId, int userId)
        {
            var course = await _context.Courses
                .Where(c => c.Id == courseId && c.IsActive)
                .Select(c => new CourseDto
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
                        ? (double)c.Lessons
                            .SelectMany(l => l.UserProgresses)
                            .Count(up => up.UserId == userId && up.Status == ProgressStatus.Completed) /
                          c.Lessons.Count(l => l.IsActive) * 100
                        : 0
                })
                .FirstOrDefaultAsync();

            return course;
        }

        public async Task<CourseDto> CreateCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

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
            var existingCourse = await _context.Courses.FindAsync(courseId);
            if (existingCourse == null) return null;

            existingCourse.Title = course.Title;
            existingCourse.Description = course.Description;
            existingCourse.ImageUrl = course.ImageUrl;
            existingCourse.Order = course.Order;
            existingCourse.IsActive = course.IsActive;
            existingCourse.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

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
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) return false;

            course.IsActive = false;
            course.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
