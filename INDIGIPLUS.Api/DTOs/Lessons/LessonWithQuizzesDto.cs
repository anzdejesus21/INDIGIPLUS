using INDIGIPLUS.Api.DTOs.Quizs;

namespace INDIGIPLUS.Api.DTOs.Lessons
{
    public class LessonWithQuizzesDto : LessonDto
    {
        #region Properties

        public List<QuizSummaryDto> Quizzes { get; set; } = new();

        #endregion Properties
    }
}