using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.DTOs.Lessons;
using INDIGIPLUS.Api.DTOs.Quizs;
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

        public async Task<IEnumerable<LessonDto>> GetAllLessonsAsync()
        {
            var lessons = await _lessonRepository.GetAllAsync();
            return lessons.Select(MapToDto);
        }

        public async Task<LessonDto?> GetLessonByIdAsync(int id)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id);
            return lesson != null ? MapToDto(lesson) : null;
        }

        public async Task<LessonWithQuizzesDto?> GetLessonWithQuizzesAsync(int id)
        {
            var lesson = await _lessonRepository.GetByIdWithQuizzesAsync(id);
            return lesson != null ? MapToLessonWithQuizzesDto(lesson) : null;
        }

        public async Task<LessonDto> CreateLessonAsync(CreateLessonDto dto)
        {
            var lesson = new Lesson
            {
                Title = dto.Title,
                Content = dto.Content,
                Description = dto.Description,
                OrderIndex = dto.OrderIndex
            };

            var createdLesson = await _lessonRepository.CreateAsync(lesson);
            return MapToDto(createdLesson);
        }

        public async Task<LessonDto?> UpdateLessonAsync(int id, UpdateLessonDto dto)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id);
            if (lesson == null)
                return null;

            lesson.Title = dto.Title;
            lesson.Content = dto.Content;
            lesson.Description = dto.Description;
            lesson.OrderIndex = dto.OrderIndex;

            var updatedLesson = await _lessonRepository.UpdateAsync(lesson);
            return MapToDto(updatedLesson);
        }

        public async Task<bool> DeleteLessonAsync(int id)
        {
            return await _lessonRepository.DeleteAsync(id);
        }

        #endregion Public Methods

        #region Private Methods

        private static LessonDto MapToDto(Lesson lesson)
        {
            return new LessonDto
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Content = lesson.Content,
                Description = lesson.Description,
                OrderIndex = lesson.OrderIndex,
                CreatedAt = lesson.CreatedAt,
                UpdatedAt = lesson.UpdatedAt
            };
        }

        private static LessonWithQuizzesDto MapToLessonWithQuizzesDto(Lesson lesson)
        {
            return new LessonWithQuizzesDto
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Content = lesson.Content,
                Description = lesson.Description,
                OrderIndex = lesson.OrderIndex,
                CreatedAt = lesson.CreatedAt,
                UpdatedAt = lesson.UpdatedAt,
                Quizzes = lesson.Quizzes.Select(q => new QuizSummaryDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    CreatedAt = q.CreatedAt
                }).ToList()
            };
        }

        #endregion Private Methods
    }
}