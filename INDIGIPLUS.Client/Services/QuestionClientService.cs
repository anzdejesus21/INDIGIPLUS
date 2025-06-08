using INDIGIPLUS.Client.DTOs.Questions;
using INDIGIPLUS.Client.Services.Interfaces;

namespace INDIGIPLUS.Client.Services
{
    public class QuestionClientService : IQuestionClientService
    {
        #region Fields

        private const string BaseUrl = "api/questions";
        private readonly HttpClient _httpClient;

        #endregion Fields

        #region Public Constructors

        public QuestionClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<List<QuestionDto>> GetAllQuestionsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<QuestionDto>>(BaseUrl);
                return response ?? new List<QuestionDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching questions: {ex.Message}");
                return new List<QuestionDto>();
            }
        }

        public async Task<List<QuestionDto>> GetQuestionsByQuizAsync(int quizId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<QuestionDto>>($"{BaseUrl}/by-quiz/{quizId}");
                return response ?? new List<QuestionDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching questions for quiz {quizId}: {ex.Message}");
                return new List<QuestionDto>();
            }
        }

        public async Task<QuestionDto?> GetQuestionByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<QuestionDto>($"{BaseUrl}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching question {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<QuestionDto?> CreateQuestionAsync(CreateQuestionDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(BaseUrl, dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<QuestionDto>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating question: {ex.Message}");
                return null;
            }
        }

        public async Task<QuestionDto?> UpdateQuestionAsync(int id, UpdateQuestionDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<QuestionDto>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating question {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting question {id}: {ex.Message}");
                return false;
            }
        }

        #endregion Public Methods
    }
}