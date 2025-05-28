namespace INDIGIPLUS.Api.DTOs
{
    public class CreateAnswerOptionDto
    {
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int Order { get; set; }
    }
}
