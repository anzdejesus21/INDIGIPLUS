using INDIGIPLUS.Api.DTOs.Answers;
using System.ComponentModel.DataAnnotations;

namespace INDIGIPLUS.Api.DTOs.Questions
{
    public class UpdateQuestionDto
    {
        #region Properties

        [Required]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        public int OrderIndex { get; set; }

        public List<CreateAnswerDto> Answers { get; set; } = new();

        #endregion Properties
    }
}