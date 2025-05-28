using INDIGIPLUS.Api.Common.Enums;
using INDIGIPLUS.Api.Data;
using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Entities;
using INDIGIPLUS.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INDIGIPLUS.Api.Services
{
    public class LessonService : ILessonService
    {
        private readonly ApplicationDbContext _context;

        public LessonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LessonDto>> GetLessonsByCourseAsync(int courseId, int userId)
        {
            var lessons = await _context.Lessons
                .Where(l => l.CourseId == courseId && l.IsActive)
                .OrderBy(l => l.Order)
                .Select(l => new LessonDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Description = l.Description,
                    Content = l.Content,
                    CodeExample = l.CodeExample,
                    Order = l.Order,
                    EstimatedMinutes = l.EstimatedMinutes,
                    Difficulty = l.Difficulty,
                    CourseId = l.CourseId,
                    CourseName = l.Course.Title,
                    HasQuiz = l.Quizzes.Any(q => q.IsActive),
                    UserProgress = l.UserProgresses
                        .Where(up => up.UserId == userId)
                        .Select(up => up.Status)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return lessons;
        }

        public async Task<LessonDto?> GetLessonByIdAsync(int lessonId, int userId)
        {
            var lesson = await _context.Lessons
                .Where(l => l.Id == lessonId && l.IsActive)
                .Select(l => new LessonDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Description = l.Description,
                    Content = l.Content,
                    CodeExample = l.CodeExample,
                    Order = l.Order,
                    EstimatedMinutes = l.EstimatedMinutes,
                    Difficulty = l.Difficulty,
                    CourseId = l.CourseId,
                    CourseName = l.Course.Title,
                    HasQuiz = l.Quizzes.Any(q => q.IsActive),
                    UserProgress = l.UserProgresses
                        .Where(up => up.UserId == userId)
                        .Select(up => up.Status)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            return lesson;
        }

        public async Task<LessonDto> CreateLessonAsync(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            var course = await _context.Courses.FindAsync(lesson.CourseId);

            return new LessonDto
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Description = lesson.Description,
                Content = lesson.Content,
                CodeExample = lesson.CodeExample,
                Order = lesson.Order,
                EstimatedMinutes = lesson.EstimatedMinutes,
                Difficulty = lesson.Difficulty,
                CourseId = lesson.CourseId,
                CourseName = course?.Title ?? "",
                HasQuiz = false,
                UserProgress = ProgressStatus.NotStarted
            };
        }

        public async Task<LessonDto?> UpdateLessonAsync(int lessonId, Lesson lesson)
        {
            var existingLesson = await _context.Lessons.FindAsync(lessonId);
            if (existingLesson == null) return null;

            existingLesson.Title = lesson.Title;
            existingLesson.Description = lesson.Description;
            existingLesson.Content = lesson.Content;
            existingLesson.CodeExample = lesson.CodeExample;
            existingLesson.Order = lesson.Order;
            existingLesson.EstimatedMinutes = lesson.EstimatedMinutes;
            existingLesson.Difficulty = lesson.Difficulty;
            existingLesson.IsActive = lesson.IsActive;
            existingLesson.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var course = await _context.Courses.FindAsync(existingLesson.CourseId);

            return new LessonDto
            {
                Id = existingLesson.Id,
                Title = existingLesson.Title,
                Description = existingLesson.Description,
                Content = existingLesson.Content,
                CodeExample = existingLesson.CodeExample,
                Order = existingLesson.Order,
                EstimatedMinutes = existingLesson.EstimatedMinutes,
                Difficulty = existingLesson.Difficulty,
                CourseId = existingLesson.CourseId,
                CourseName = course?.Title ?? ""
            };
        }

        public async Task<bool> DeleteLessonAsync(int lessonId)
        {
            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson == null) return false;

            lesson.IsActive = false;
            lesson.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MarkLessonAsStartedAsync(int lessonId, int userId)
        {
            var progress = await _context.UserProgresses
                .FirstOrDefaultAsync(up => up.LessonId == lessonId && up.UserId == userId);

            if (progress == null)
            {
                progress = new UserProgress
                {
                    LessonId = lessonId,
                    UserId = userId,
                    Status = ProgressStatus.InProgress,
                    StartedAt = DateTime.UtcNow,
                    LastAccessedAt = DateTime.UtcNow
                };
                _context.UserProgresses.Add(progress);
            }
            else if (progress.Status == ProgressStatus.NotStarted)
            {
                progress.Status = ProgressStatus.InProgress;
                progress.StartedAt = DateTime.UtcNow;
                progress.LastAccessedAt = DateTime.UtcNow;
            }
            else
            {
                progress.LastAccessedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkLessonAsCompletedAsync(int lessonId, int userId)
        {
            var progress = await _context.UserProgresses
                .FirstOrDefaultAsync(up => up.LessonId == lessonId && up.UserId == userId);

            if (progress == null)
            {
                progress = new UserProgress
                {
                    LessonId = lessonId,
                    UserId = userId,
                    Status = ProgressStatus.Completed,
                    StartedAt = DateTime.UtcNow,
                    CompletedAt = DateTime.UtcNow,
                    CompletionPercentage = 100,
                    LastAccessedAt = DateTime.UtcNow
                };
                _context.UserProgresses.Add(progress);
            }
            else
            {
                progress.Status = ProgressStatus.Completed;
                progress.CompletedAt = DateTime.UtcNow;
                progress.CompletionPercentage = 100;
                progress.LastAccessedAt = DateTime.UtcNow;

                if (progress.StartedAt.HasValue)
                {
                    progress.TimeSpent = DateTime.UtcNow - progress.StartedAt.Value;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
