using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.DTOs.Lessons;

namespace INDIGIPLUS.Api.Services.Interfaces
{
    public interface ILessonService
    {
        #region Public Methods

        Task<IEnumerable<LessonDto>> GetAllLessonsAsync();

        Task<LessonDto?> GetLessonByIdAsync(int id);

        Task<LessonWithQuizzesDto?> GetLessonWithQuizzesAsync(int id);

        Task<LessonDto> CreateLessonAsync(CreateLessonDto dto);

        Task<LessonDto?> UpdateLessonAsync(int id, UpdateLessonDto dto);

        Task<bool> DeleteLessonAsync(int id);

        #endregion Public Methods
    }
}