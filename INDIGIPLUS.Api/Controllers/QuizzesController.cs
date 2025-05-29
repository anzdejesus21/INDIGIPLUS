using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.DTOs.Quizs;
using INDIGIPLUS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace INDIGIPLUS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizzesController : ControllerBase
    {
        #region Fields

        private readonly IQuizService _quizService;

        #endregion Fields

        #region Public Constructors

        public QuizzesController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetQuizzes()
        {
            var quizzes = await _quizService.GetAllQuizzesAsync();
            return Ok(quizzes);
        }

        [HttpGet("by-lesson/{lessonId}")]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetQuizzesByLesson(int lessonId)
        {
            var quizzes = await _quizService.GetQuizzesByLessonIdAsync(lessonId);
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDto>> GetQuiz(int id)
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);
            if (quiz == null)
                return NotFound();

            return Ok(quiz);
        }

        [HttpGet("{id}/with-questions")]
        public async Task<ActionResult<QuizWithQuestionsDto>> GetQuizWithQuestions(int id)
        {
            var quiz = await _quizService.GetQuizWithQuestionsAsync(id);
            if (quiz == null)
                return NotFound();

            return Ok(quiz);
        }

        [HttpPost]
        public async Task<ActionResult<QuizDto>> CreateQuiz(CreateQuizDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var quiz = await _quizService.CreateQuizAsync(dto);
            return CreatedAtAction(nameof(GetQuiz), new { id = quiz.Id }, quiz);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<QuizDto>> UpdateQuiz(int id, UpdateQuizDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var quiz = await _quizService.UpdateQuizAsync(id, dto);
            if (quiz == null)
                return NotFound();

            return Ok(quiz);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var result = await _quizService.DeleteQuizAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        #endregion Public Methods
    }
}