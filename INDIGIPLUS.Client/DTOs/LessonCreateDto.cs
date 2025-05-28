namespace INDIGIPLUS.Client.DTOs
{
    public class LessonCreateDto
    {
        #region Properties

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? CodeExample { get; set; }
        public int Order { get; set; }
        public int EstimatedMinutes { get; set; }
        public string Difficulty { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public bool IsActive { get; set; } = true;

        #endregion Properties
    }
}