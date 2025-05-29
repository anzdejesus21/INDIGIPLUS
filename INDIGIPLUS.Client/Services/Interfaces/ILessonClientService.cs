using INDIGIPLUS.Client.DTOs.Lessons;
using System.Buffers.Text;
using System.Net.Http;

namespace INDIGIPLUS.Client.Services.Interfaces
{
    public interface ILessonClientService
    {
        #region Public Methods

        Task<List<LessonDto>> GetAllLessonsAsync();

        Task<LessonDto?> GetLessonByIdAsync(int id);

        Task<LessonWithQuizzesDto?> GetLessonWithQuizzesAsync(int id);

        Task<LessonDto?> CreateLessonAsync(CreateLessonDto dto);

        Task<LessonDto?> UpdateLessonAsync(int id, UpdateLessonDto dto);

        Task<bool> DeleteLessonAsync(int id);

        #endregion Public Methods
    }
}