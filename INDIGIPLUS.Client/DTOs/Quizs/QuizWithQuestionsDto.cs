using INDIGIPLUS.Client.DTOs.Questions;

namespace INDIGIPLUS.Client.DTOs.Quizs
{
    public class QuizWithQuestionsDto : QuizDto
    {
        #region Properties

        public List<QuestionDto> Questions { get; set; } = new();

        #endregion Properties
    }
}