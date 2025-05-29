namespace INDIGIPLUS.Client.Models
{
    public class QuizFormModel
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int LessonId { get; set; }

        #endregion Properties
    }
}