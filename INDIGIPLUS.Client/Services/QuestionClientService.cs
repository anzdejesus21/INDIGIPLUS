﻿using INDIGIPLUS.Client.DTOs.Questions;
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
                Console.WriteLine($"Sending POST request to: {BaseUrl}");
                Console.WriteLine($"Data: QuizId={dto.QuizId}, QuestionText='{dto.QuestionText}', Type={dto.Type}, Answers={dto.Answers.Count}");

                var response = await _httpClient.PostAsJsonAsync(BaseUrl, dto);

                Console.WriteLine($"Response Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<QuestionDto>();
                    Console.WriteLine("Question created successfully");
                    return result;
                }
                else
                {
                    // Read the error response for debugging
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error ({response.StatusCode}): {errorContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception creating question: {ex}");
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
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error updating question ({response.StatusCode}): {errorContent}");
                    return null;
                }
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