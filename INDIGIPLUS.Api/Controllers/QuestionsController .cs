using INDIGIPLUS.Api.Data;
using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace INDIGIPLUS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Question>> CreateQuestion([FromBody] CreateQuestionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var question = new Question
            {
                QuestionText = dto.QuestionText,
                CodeSnippet = dto.CodeSnippet,
                Type = dto.Type,
                Points = dto.Points,
                Order = dto.Order,
                Explanation = dto.Explanation,
                QuizId = dto.QuizId
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            // Add answer options if provided
            if (dto.AnswerOptions?.Any() == true)
            {
                foreach (var optionDto in dto.AnswerOptions)
                {
                    var option = new AnswerOption
                    {
                        OptionText = optionDto.OptionText,
                        IsCorrect = optionDto.IsCorrect,
                        Order = optionDto.Order,
                        QuestionId = question.Id
                    };
                    _context.AnswerOptions.Add(option);
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _context.Questions
                .Include(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null) return NotFound();

            return Ok(question);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Question>> UpdateQuestion(int id, [FromBody] UpdateQuestionDto dto)
        {
            var question = await _context.Questions
                .Include(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null) return NotFound();

            question.QuestionText = dto.QuestionText;
            question.CodeSnippet = dto.CodeSnippet;
            question.Type = dto.Type;
            question.Points = dto.Points;
            question.Order = dto.Order;
            question.Explanation = dto.Explanation;
            question.IsActive = dto.IsActive;

            // Update answer options
            if (dto.AnswerOptions?.Any() == true)
            {
                // Remove existing options
                _context.AnswerOptions.RemoveRange(question.AnswerOptions);

                // Add new options
                foreach (var optionDto in dto.AnswerOptions)
                {
                    var option = new AnswerOption
                    {
                        OptionText = optionDto.OptionText,
                        IsCorrect = optionDto.IsCorrect,
                        Order = optionDto.Order,
                        QuestionId = question.Id
                    };
                    _context.AnswerOptions.Add(option);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(question);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return NotFound();

            question.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
