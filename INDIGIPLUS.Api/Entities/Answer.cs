using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Api.Entities
{
    public class Answer
    {
        #region Properties

        public int Id { get; set; }

        [Required]
        public string AnswerText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }

        // Navigation properties
        public virtual Question Question { get; set; } = null!;

        #endregion Properties
    }
}