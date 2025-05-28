using INDIGIPLUS.Client.Common.Enums;

namespace INDIGIPLUS.Client.Models
{
    public class LessonEntity
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Content { get; set; } = "";
        public string CodeExample { get; set; } = "";
        public int Order { get; set; }
        public int EstimatedMinutes { get; set; }
        public Difficulty LessonDifficulty { get; set; }
        public ProgressStatus ProgressStatus { get; set; }
        public int CourseId { get; set; }
        public bool IsActive { get; set; } = true;

        #endregion Properties
    }
}