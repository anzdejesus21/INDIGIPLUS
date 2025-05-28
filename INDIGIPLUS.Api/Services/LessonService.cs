using INDIGIPLUS.Api.Common.Enums;
using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Entities;
using INDIGIPLUS.Api.Repositories.Interfaces;
using INDIGIPLUS.Api.Services.Interfaces;

namespace INDIGIPLUS.Api.Services
{
    public class LessonService : ILessonService
    {
        #region Fields

        private readonly ILessonRepository _lessonRepository;

        #endregion Fields

        #region Public Constructors

        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<List<LessonDto>> GetLessonsByCourseAsync(int courseId, int userId)
        {
            var lessons = await _lessonRepository.GetLessonsByCourseAsync(courseId);

            return lessons.Select(l => new LessonDto
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
                CourseName = l.Course?.Title ?? "",
                HasQuiz = l.Quizzes.Any(q => q.IsActive),
                UserProgress = l.UserProgresses
                    .Where(up => up.UserId == userId)
                    .Select(up => up.Status)
                    .FirstOrDefault()
            }).ToList();
        }

        public async Task<LessonDto?> GetLessonByIdAsync(int lessonId, int userId)
        {
            var l = await _lessonRepository.GetLessonByIdAsync(lessonId);
            if (l == null)
                return null;

            return new LessonDto
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
                CourseName = l.Course?.Title ?? "",
                HasQuiz = l.Quizzes.Any(q => q.IsActive),
                UserProgress = l.UserProgresses
                    .Where(up => up.UserId == userId)
                    .Select(up => up.Status)
                    .FirstOrDefault()
            };
        }

        public async Task<LessonDto> CreateLessonAsync(Lesson lesson)
        {
            await _lessonRepository.AddLessonAsync(lesson);
            await _lessonRepository.SaveChangesAsync();

            var courseTitle = lesson.Course?.Title ?? "";

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
                CourseName = courseTitle,
                HasQuiz = false,
                UserProgress = ProgressStatus.NotStarted
            };
        }

        public async Task<LessonDto?> UpdateLessonAsync(int lessonId, Lesson lesson)
        {
            var existingLesson = await _lessonRepository.GetLessonByIdAsync(lessonId);
            if (existingLesson == null)
                return null;

            existingLesson.Title = lesson.Title;
            existingLesson.Description = lesson.Description;
            existingLesson.Content = lesson.Content;
            existingLesson.CodeExample = lesson.CodeExample;
            existingLesson.Order = lesson.Order;
            existingLesson.EstimatedMinutes = lesson.EstimatedMinutes;
            existingLesson.Difficulty = lesson.Difficulty;
            existingLesson.IsActive = lesson.IsActive;
            existingLesson.UpdatedAt = DateTime.UtcNow;

            await _lessonRepository.UpdateLessonAsync(existingLesson);
            await _lessonRepository.SaveChangesAsync();

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
                CourseName = existingLesson.Course?.Title ?? ""
            };
        }

        public async Task<bool> DeleteLessonAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetLessonByIdAsync(lessonId);
            if (lesson == null)
                return false;

            await _lessonRepository.DeleteLessonAsync(lesson);
            await _lessonRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MarkLessonAsStartedAsync(int lessonId, int userId)
        {
            var progress = await _lessonRepository.GetUserProgressAsync(lessonId, userId);

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
                await _lessonRepository.AddUserProgressAsync(progress);
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

            await _lessonRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkLessonAsCompletedAsync(int lessonId, int userId)
        {
            var progress = await _lessonRepository.GetUserProgressAsync(lessonId, userId);

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
                await _lessonRepository.AddUserProgressAsync(progress);
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

            await _lessonRepository.SaveChangesAsync();
            return true;
        }

        #endregion Public Methods
    }
}