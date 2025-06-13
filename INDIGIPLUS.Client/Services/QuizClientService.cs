using INDIGIPLUS.Client.DTOs.Quizs;
using INDIGIPLUS.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace INDIGIPLUS.Client.Services
{
    public class QuizClientService : IQuizClientService
    {
        #region Fields

        private const string BaseUrl = "api/quizzes";
        private readonly HttpClient _httpClient;

        #endregion Fields

        #region Public Constructors

        public QuizClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<List<QuizDto>> GetAllQuizzesAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<QuizDto>>(BaseUrl);
                return response ?? new List<QuizDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching quizzes: {ex.Message}");
                return new List<QuizDto>();
            }
        }

        public async Task<List<QuizDto>> GetQuizzesByLessonAsync(int lessonId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<QuizDto>>($"{BaseUrl}/by-lesson/{lessonId}");
                return response ?? new List<QuizDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching quizzes for lesson {lessonId}: {ex.Message}");
                return new List<QuizDto>();
            }
        }

        public async Task<QuizDto?> GetQuizByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<QuizDto>($"{BaseUrl}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching quiz {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<QuizDto?> CreateQuizAsync(CreateQuizDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(BaseUrl, dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<QuizDto>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating quiz: {ex.Message}");
                return null;
            }
        }

        public async Task<QuizDto?> UpdateQuizAsync(int id, UpdateQuizDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<QuizDto>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating quiz {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteQuizAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting quiz {id}: {ex.Message}");
                return false;
            }
        }

        public Task<QuizWithQuestionsDto?> GetQuizWithQuestionsAsync(int id)
        {
            throw new NotImplementedException();
        }


        #endregion Public Methods
    }
}
