namespace INDIGIPLUS.Client.Models
{
    public class LessonFormModel
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int OrderIndex { get; set; }

        #endregion Properties
    }
}