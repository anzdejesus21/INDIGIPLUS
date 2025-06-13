using INDIGIPLUS.Client.DTOs.Questions;

namespace INDIGIPLUS.Client.DTOs.Quizs
{
    public class QuizWithQuestionsDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int LessonId { get; set; }

        public List<CreateQuestionDto> Questions { get; set; } = new();
    }
}
