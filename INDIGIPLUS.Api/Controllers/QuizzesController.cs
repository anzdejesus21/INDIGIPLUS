using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Entities;
using INDIGIPLUS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace INDIGIPLUS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizzesController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        [HttpGet("lesson/{lessonId}")]
        public async Task<ActionResult<List<QuizDto>>> GetQuizzesByLesson(int lessonId)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var quizzes = await _quizService.GetQuizzesByLessonAsync(lessonId, userId);
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDto>> GetQuiz(int id)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var quiz = await _quizService.GetQuizByIdAsync(id, userId);
            if (quiz == null) return NotFound();

            return Ok(quiz);
        }

        [HttpGet("{id}/questions")]
        public async Task<ActionResult<List<QuestionDto>>> GetQuizQuestions(int id)
        {
            var questions = await _quizService.GetQuizQuestionsAsync(id);
            return Ok(questions);
        }

        [HttpPost("submit")]
        public async Task<ActionResult<QuizResultDto>> SubmitQuiz([FromBody] QuizSubmissionDto submission)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _quizService.SubmitQuizAsync(userId, submission);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/attempts")]
        public async Task<ActionResult<List<QuizResultDto>>> GetQuizAttempts(int id)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var attempts = await _quizService.GetUserQuizAttemptsAsync(userId, id);
            return Ok(attempts);
        }

        [HttpPost]
        public async Task<ActionResult<QuizDto>> CreateQuiz([FromBody] Quiz quiz)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdQuiz = await _quizService.CreateQuizAsync(quiz);
            return CreatedAtAction(nameof(GetQuiz), new { id = createdQuiz.Id }, createdQuiz);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<QuizDto>> UpdateQuiz(int id, [FromBody] Quiz quiz)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedQuiz = await _quizService.UpdateQuizAsync(id, quiz);
            if (updatedQuiz == null) return NotFound();

            return Ok(updatedQuiz);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuiz(int id)
        {
            var result = await _quizService.DeleteQuizAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
