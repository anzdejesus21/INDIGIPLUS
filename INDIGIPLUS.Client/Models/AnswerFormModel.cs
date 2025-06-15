using INDIGIPLUS.Client.DTOs.Questions;
using INDIGIPLUS.Client.Services.Interfaces;

namespace INDIGIPLUS.Client.Models
{
    public class AnswerFormModel
    {
        #region Properties
        public int Id { get; set; }
        public string AnswerText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public string? Explanation { get; set; }
        public int OrderIndex { get; set; }
        #endregion Properties
    }
}