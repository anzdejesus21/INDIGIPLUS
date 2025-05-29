using INDIGIPLUS.Api.DTOs.Questions;

namespace INDIGIPLUS.Api.DTOs.Quizs
{
    public class QuizWithQuestionsDto : QuizDto
    {
        #region Properties

        public List<QuestionDto> Questions { get; set; } = new();

        #endregion Properties
    }
}