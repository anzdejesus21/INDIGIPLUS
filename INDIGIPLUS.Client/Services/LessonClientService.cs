using INDIGIPLUS.Client.DTOs;
using INDIGIPLUS.Client.Services.Interfaces;
using System.Text.Json;

namespace INDIGIPLUS.Client.Services
{
    public class LessonClientService : ILessonClientService
    {
        #region Fields

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        #endregion Fields

        #region Public Constructors

        public LessonClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        #endregion Public Constructors

        #region Lesson Methods

        public async Task<List<LessonDto>> GetLessonsByCourseAsync(int courseId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/lessons/course/{courseId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<LessonDto>>(_jsonOptions) ?? new List<LessonDto>();
            }
            catch (HttpRequestException)
            {
                return new List<LessonDto>();
            }
        }

        public async Task<LessonDto?> GetLessonByIdAsync(int lessonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/lessons/{lessonId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LessonDto>(_jsonOptions);
                }
                return null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<LessonDto> CreateLessonAsync(LessonDto lesson)
        {
            var response = await _httpClient.PostAsJsonAsync("api/lessons", lesson, _jsonOptions);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<LessonDto>(_jsonOptions) ?? throw new InvalidOperationException("Failed to create lesson");
        }

        public async Task<LessonDto?> UpdateLessonAsync(int lessonId, LessonDto lesson)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/lessons/{lessonId}", lesson, _jsonOptions);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LessonDto>(_jsonOptions);
                }
                return null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<bool> DeleteLessonAsync(int lessonId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/lessons/{lessonId}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        public async Task<bool> StartLessonAsync(int lessonId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/lessons/{lessonId}/start", null);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        public async Task<bool> CompleteLessonAsync(int lessonId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/lessons/{lessonId}/complete", null);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        #endregion Lesson Methods

        #region Course Methods

        public async Task<List<CourseDto>> GetCoursesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/courses");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<CourseDto>>(_jsonOptions) ?? new List<CourseDto>();
            }
            catch (HttpRequestException)
            {
                return new List<CourseDto>();
            }
        }

        public async Task<CourseDto?> GetCourseByIdAsync(int courseId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/courses/{courseId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CourseDto>(_jsonOptions);
                }
                return null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<CourseDto> CreateCourseAsync(CourseDto course)
        {
            var response = await _httpClient.PostAsJsonAsync("api/courses", course, _jsonOptions);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CourseDto>(_jsonOptions) ?? throw new InvalidOperationException("Failed to create course");
        }

        public async Task<CourseDto?> UpdateCourseAsync(int courseId, CourseDto course)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/courses/{courseId}", course, _jsonOptions);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CourseDto>(_jsonOptions);
                }
                return null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/courses/{courseId}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        #endregion Course Methods
    }
}