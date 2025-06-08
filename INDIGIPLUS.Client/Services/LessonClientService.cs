using INDIGIPLUS.Client.DTOs.Lessons;
using INDIGIPLUS.Client.Services.Interfaces;

namespace INDIGIPLUS.Client.Services
{
    public class LessonClientService : ILessonClientService
    {
        #region Fields

        private readonly HttpClient _httpClient;

        #endregion Fields

        #region Public Constructors

        public LessonClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<List<LessonDto>> GetAllLessonsAsync()
        {
            try
            {
                Console.WriteLine("Making API call to get-lessons...");
                Console.WriteLine($"HttpClient BaseAddress: {_httpClient.BaseAddress}");
                Console.WriteLine($"Authorization Header: {_httpClient.DefaultRequestHeaders.Authorization?.ToString() ?? "None"}");

                var response = await _httpClient.GetAsync("api/lessons/get-lessons");

                Console.WriteLine($"API Response Status: {response.StatusCode}");
                Console.WriteLine($"API Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error Content: {errorContent}");
                    return new List<LessonDto>();
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response Content: {responseContent}");

                var result = await response.Content.ReadFromJsonAsync<List<LessonDto>>();
                Console.WriteLine($"Deserialized {result?.Count ?? 0} lessons");

                return result ?? new List<LessonDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetAllLessonsAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<LessonDto>();
            }
        }

        public async Task<LessonDto?> GetLessonByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<LessonDto>($"api/lessons/get-lesson-by-id/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching lesson {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<LessonWithQuizzesDto?> GetLessonWithQuizzesAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<LessonWithQuizzesDto>($"api/lessons/get-lessons/with-quizzes/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching lesson with quizzes {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<LessonDto?> CreateLessonAsync(CreateLessonDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/lessons/create-lessons", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LessonDto>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating lesson: {ex.Message}");
                return null;
            }
        }

        public async Task<LessonDto?> UpdateLessonAsync(int id, UpdateLessonDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/lessons/update-lessons/{id}", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LessonDto>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating lesson {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteLessonAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/lessons/delete-lesson/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting lesson {id}: {ex.Message}");
                return false;
            }
        }

        #endregion Public Methods
    }
}