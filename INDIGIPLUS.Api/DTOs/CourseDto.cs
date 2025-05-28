namespace INDIGIPLUS.Api.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Order { get; set; }
        public int LessonCount { get; set; }
        public int CompletedLessons { get; set; }
        public double ProgressPercentage { get; set; }
    }
}
