using INDIGIPLUS.Client.DTOs.Quizs;

namespace INDIGIPLUS.Client.DTOs.Lessons
{
    public class LessonWithQuizzesDto : LessonDto
    {
        #region Properties

        public List<QuizSummaryDto> Quizzes { get; set; } = new();

        #endregion Properties
    }
}