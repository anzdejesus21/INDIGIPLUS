using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.DTOs.Answers;
using INDIGIPLUS.Api.DTOs.Questions;
using INDIGIPLUS.Api.DTOs.Quizs;
using INDIGIPLUS.Api.Entities;
using INDIGIPLUS.Api.Repositories.Interfaces;
using INDIGIPLUS.Api.Services.Interfaces;

namespace INDIGIPLUS.Api.Services
{
    public class QuizService : IQuizService
    {
        #region Fields

        private readonly IQuizRepository _quizRepository;

        #endregion Fields

        #region Public Constructors

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<IEnumerable<QuizDto>> GetAllQuizzesAsync()
        {
            var quizzes = await _quizRepository.GetAllAsync();
            return quizzes.Select(MapToDto);
        }

        public async Task<IEnumerable<QuizDto>> GetQuizzesByLessonIdAsync(int lessonId)
        {
            var quizzes = await _quizRepository.GetByLessonIdAsync(lessonId);
            return quizzes.Select(MapToDto);
        }

        public async Task<QuizDto?> GetQuizByIdAsync(int id)
        {
            var quiz = await _quizRepository.GetByIdAsync(id);
            return quiz != null ? MapToDto(quiz) : null;
        }

        public async Task<QuizWithQuestionsDto?> GetQuizWithQuestionsAsync(int id)
        {
            var quiz = await _quizRepository.GetByIdWithQuestionsAsync(id);
            return quiz != null ? MapToQuizWithQuestionsDto(quiz) : null;
        }

        public async Task<QuizDto> CreateQuizAsync(CreateQuizDto dto)
        {
            var quiz = new Quiz
            {
                Title = dto.Title,
                Description = dto.Description,
                LessonId = dto.LessonId
            };

            var createdQuiz = await _quizRepository.CreateAsync(quiz);
            return MapToDto(createdQuiz);
        }

        public async Task<QuizDto?> UpdateQuizAsync(int id, UpdateQuizDto dto)
        {
            var quiz = await _quizRepository.GetByIdAsync(id);
            if (quiz == null)
                return null;

            quiz.Title = dto.Title;
            quiz.Description = dto.Description;
            quiz.LessonId = dto.LessonId;

            var updatedQuiz = await _quizRepository.UpdateAsync(quiz);
            return MapToDto(updatedQuiz);
        }

        public async Task<bool> DeleteQuizAsync(int id)
        {
            return await _quizRepository.DeleteAsync(id);
        }

        #endregion Public Methods

        #region Private Methods

        private static QuizDto MapToDto(Quiz quiz)
        {
            return new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                LessonId = quiz.LessonId,
                LessonTitle = quiz.Lesson?.Title ?? string.Empty,
                CreatedAt = quiz.CreatedAt,
                UpdatedAt = quiz.UpdatedAt
            };
        }

        private static QuizWithQuestionsDto MapToQuizWithQuestionsDto(Quiz quiz)
        {
            return new QuizWithQuestionsDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                LessonId = quiz.LessonId,
                LessonTitle = quiz.Lesson?.Title ?? string.Empty,
                CreatedAt = quiz.CreatedAt,
                UpdatedAt = quiz.UpdatedAt,
                Questions = quiz.Questions.OrderBy(q => q.OrderIndex).Select(q => new QuestionDto
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    Type = q.Type.ToString(),
                    QuizId = q.QuizId,
                    OrderIndex = q.OrderIndex,
                    Answers = q.Answers.Select(a => new AnswerDto
                    {
                        Id = a.Id,
                        AnswerText = a.AnswerText,
                        IsCorrect = a.IsCorrect,
                        QuestionId = a.QuestionId
                    }).ToList()
                }).ToList()
            };
        }

        #endregion Private Methods
    }
}