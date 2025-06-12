using INDIGIPLUS.Client.Common.Enums;

namespace INDIGIPLUS.Client.Models
{
    public class QuestionFormModel
    {
        #region Properties
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
        public int QuizId { get; set; }
        public int OrderIndex { get; set; }
        public List<AnswerFormModel> Answers { get; set; } = new();
        #endregion Properties
    }

    
}