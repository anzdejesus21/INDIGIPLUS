using INDIGIPLUS.Api.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Api.Entities
{
    public class Question
    {
        #region Properties

        public int Id { get; set; }

        [Required]
        public string QuestionText { get; set; } = string.Empty;

        public QuestionType Type { get; set; } = QuestionType.MultipleChoice;

        public int QuizId { get; set; }

        public int OrderIndex { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Quiz Quiz { get; set; } = null!;

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

        #endregion Properties
    }
}