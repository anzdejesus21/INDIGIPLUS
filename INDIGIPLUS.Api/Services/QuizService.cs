using INDIGIPLUS.Api.Common.Enums;
using INDIGIPLUS.Api.Data;
using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Entities;
using INDIGIPLUS.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INDIGIPLUS.Api.Services
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext _context;

        public QuizService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<QuizDto>> GetQuizzesByLessonAsync(int lessonId, int userId)
        {
            var quizzes = await _context.Quizzes
                .Where(q => q.LessonId == lessonId && q.IsActive)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    TimeLimit = q.TimeLimit,
                    PassingScore = q.PassingScore,
                    LessonId = q.LessonId,
                    QuestionCount = q.Questions.Count(qu => qu.IsActive),
                    BestScore = q.QuizAttempts
                        .Where(qa => qa.UserId == userId)
                        .Max(qa => (int?)qa.Score),
                    AttemptCount = q.QuizAttempts.Count(qa => qa.UserId == userId),
                    IsPassed = q.QuizAttempts
                        .Any(qa => qa.UserId == userId && qa.IsPassed)
                })
                .ToListAsync();

            return quizzes;
        }

        public async Task<QuizDto?> GetQuizByIdAsync(int quizId, int userId)
        {
            var quiz = await _context.Quizzes
                .Where(q => q.Id == quizId && q.IsActive)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    TimeLimit = q.TimeLimit,
                    PassingScore = q.PassingScore,
                    LessonId = q.LessonId,
                    QuestionCount = q.Questions.Count(qu => qu.IsActive),
                    BestScore = q.QuizAttempts
                        .Where(qa => qa.UserId == userId)
                        .Max(qa => (int?)qa.Score),
                    AttemptCount = q.QuizAttempts.Count(qa => qa.UserId == userId),
                    IsPassed = q.QuizAttempts
                        .Any(qa => qa.UserId == userId && qa.IsPassed)
                })
                .FirstOrDefaultAsync();

            return quiz;
        }

        public async Task<List<QuestionDto>> GetQuizQuestionsAsync(int quizId)
        {
            var questions = await _context.Questions
                .Where(q => q.QuizId == quizId && q.IsActive)
                .OrderBy(q => q.Order)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    CodeSnippet = q.CodeSnippet,
                    Type = q.Type,
                    Points = q.Points,
                    Order = q.Order,
                    AnswerOptions = q.AnswerOptions
                        .OrderBy(ao => ao.Order)
                        .Select(ao => new AnswerOptionDto
                        {
                            Id = ao.Id,
                            OptionText = ao.OptionText,
                            Order = ao.Order
                        }).ToList()
                })
                .ToListAsync();

            return questions;
        }

        public async Task<QuizResultDto> SubmitQuizAsync(int userId, QuizSubmissionDto submission)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(qu => qu.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == submission.QuizId && q.IsActive);

            if (quiz == null)
                throw new InvalidOperationException("Quiz not found");

            // Create quiz attempt
            var attempt = new QuizAttempt
            {
                UserId = userId,
                QuizId = submission.QuizId,
                StartedAt = DateTime.UtcNow
            };

            _context.QuizAttempts.Add(attempt);
            await _context.SaveChangesAsync();

            var questionResults = new List<QuestionResultDto>();
            int totalScore = 0;
            int totalPoints = 0;

            // Process each answer
            foreach (var question in quiz.Questions.Where(q => q.IsActive))
            {
                totalPoints += question.Points;
                var userSubmission = submission.Answers.FirstOrDefault(a => a.QuestionId == question.Id);

                var userAnswer = new UserAnswer
                {
                    QuizAttemptId = attempt.Id,
                    QuestionId = question.Id,
                    AnsweredAt = DateTime.UtcNow
                };

                bool isCorrect = false;
                string? correctAnswer = null;
                string? userAnswerText = null;

                if (question.Type == QuestionType.MultipleChoice || question.Type == QuestionType.TrueFalse)
                {
                    if (userSubmission?.SelectedAnswerOptionId.HasValue == true)
                    {
                        var selectedOption = question.AnswerOptions
                            .FirstOrDefault(ao => ao.Id == userSubmission.SelectedAnswerOptionId);

                        if (selectedOption != null)
                        {
                            userAnswer.SelectedAnswerOptionId = selectedOption.Id;
                            isCorrect = selectedOption.IsCorrect;
                            userAnswerText = selectedOption.OptionText;
                        }
                    }

                    var correctOption = question.AnswerOptions.FirstOrDefault(ao => ao.IsCorrect);
                    correctAnswer = correctOption?.OptionText;
                }
                else if (question.Type == QuestionType.ShortAnswer || question.Type == QuestionType.CodeCompletion)
                {
                    userAnswer.AnswerText = userSubmission?.AnswerText;
                    userAnswerText = userSubmission?.AnswerText;

                    // For text answers, you might want to implement fuzzy matching or manual grading
                    // For now, we'll mark them as requiring manual review
                    isCorrect = false; // Manual review required
                }

                userAnswer.IsCorrect = isCorrect;
                userAnswer.PointsEarned = isCorrect ? question.Points : 0;
                totalScore += userAnswer.PointsEarned;

                _context.UserAnswers.Add(userAnswer);

                questionResults.Add(new QuestionResultDto
                {
                    QuestionId = question.Id,
                    QuestionText = question.QuestionText,
                    IsCorrect = isCorrect,
                    PointsEarned = userAnswer.PointsEarned,
                    UserAnswer = userAnswerText,
                    CorrectAnswer = correctAnswer,
                    Explanation = question.Explanation
                });
            }

            attempt.CompletedAt = DateTime.UtcNow;
            attempt.Duration = attempt.CompletedAt.Value - attempt.StartedAt;
            attempt.Score = totalScore;
            attempt.IsPassed = totalScore >= quiz.PassingScore;

            await _context.SaveChangesAsync();

            return new QuizResultDto
            {
                AttemptId = attempt.Id,
                Score = totalScore,
                TotalPoints = totalPoints,
                Percentage = totalPoints == 0 ? 0 : (double)totalScore / totalPoints * 100,
                IsPassed = attempt.IsPassed,
                Duration = attempt.Duration ?? TimeSpan.Zero,
                QuestionResults = questionResults
            };
        }

        public async Task<List<QuizResultDto>> GetUserQuizAttemptsAsync(int userId, int quizId)
        {
            var attempts = await _context.QuizAttempts
                .Where(qa => qa.UserId == userId && qa.QuizId == quizId)
                .Include(qa => qa.UserAnswers)
                .ThenInclude(ua => ua.Question)
                .ThenInclude(q => q.AnswerOptions)
                .OrderByDescending(qa => qa.StartedAt)
                .ToListAsync();

            var resultList = new List<QuizResultDto>();

            foreach (var attempt in attempts)
            {
                var questions = attempt.UserAnswers.Select(ua =>
                {
                    var question = ua.Question;
                    var correctAnswer = question.AnswerOptions.FirstOrDefault(a => a.IsCorrect)?.OptionText;
                    var userAnswerText = question.Type == QuestionType.MultipleChoice || question.Type == QuestionType.TrueFalse
                        ? question.AnswerOptions.FirstOrDefault(a => a.Id == ua.SelectedAnswerOptionId)?.OptionText
                        : ua.AnswerText;

                    return new QuestionResultDto
                    {
                        QuestionId = question.Id,
                        QuestionText = question.QuestionText,
                        IsCorrect = ua.IsCorrect,
                        PointsEarned = ua.PointsEarned,
                        UserAnswer = userAnswerText,
                        CorrectAnswer = correctAnswer,
                        Explanation = question.Explanation
                    };
                }).ToList();

                var totalPoints = attempt.UserAnswers.Sum(ua => ua.Question.Points);

                resultList.Add(new QuizResultDto
                {
                    AttemptId = attempt.Id,
                    Score = attempt.Score,
                    TotalPoints = totalPoints,
                    Percentage = totalPoints == 0 ? 0 : (double)attempt.Score / totalPoints * 100,
                    IsPassed = attempt.IsPassed,
                    Duration = attempt.Duration ?? TimeSpan.Zero,
                    QuestionResults = questions
                });
            }

            return resultList;
        }

        public async Task<QuizDto> CreateQuizAsync(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            return new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                TimeLimit = quiz.TimeLimit,
                PassingScore = quiz.PassingScore,
                LessonId = quiz.LessonId,
                QuestionCount = 0,
                BestScore = null,
                AttemptCount = 0,
                IsPassed = false
            };
        }

        public async Task<QuizDto?> UpdateQuizAsync(int quizId, Quiz updatedQuiz)
        {
            var existingQuiz = await _context.Quizzes.FindAsync(quizId);

            if (existingQuiz == null || !existingQuiz.IsActive)
                return null;

            existingQuiz.Title = updatedQuiz.Title;
            existingQuiz.Description = updatedQuiz.Description;
            existingQuiz.TimeLimit = updatedQuiz.TimeLimit;
            existingQuiz.PassingScore = updatedQuiz.PassingScore;
            existingQuiz.LessonId = updatedQuiz.LessonId;

            await _context.SaveChangesAsync();

            return new QuizDto
            {
                Id = existingQuiz.Id,
                Title = existingQuiz.Title,
                Description = existingQuiz.Description,
                TimeLimit = existingQuiz.TimeLimit,
                PassingScore = existingQuiz.PassingScore,
                LessonId = existingQuiz.LessonId,
                QuestionCount = await _context.Questions.CountAsync(q => q.QuizId == existingQuiz.Id && q.IsActive),
                BestScore = null,
                AttemptCount = 0,
                IsPassed = false
            };
        }

        public async Task<bool> DeleteQuizAsync(int quizId)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz == null || !quiz.IsActive)
                return false;

            quiz.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
