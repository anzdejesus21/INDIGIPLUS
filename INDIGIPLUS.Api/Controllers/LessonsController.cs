using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.DTOs.Lessons;
using INDIGIPLUS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INDIGIPLUS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]

    public class LessonsController : ControllerBase
    {
        #region Fields

        private readonly ILessonService _lessonService;

        #endregion Fields

        #region Public Constructors

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpGet("get-lessons")]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetLessons()
        {
            var lessons = await _lessonService.GetAllLessonsAsync();
            return Ok(lessons);
        }

        [HttpGet("get-lesson-by-id/{id}")]
        public async Task<ActionResult<LessonDto>> GetLesson(int id)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id);
            if (lesson == null)
                return NotFound();

            return Ok(lesson);
        }

        [HttpGet("api/lessons/get-lessons/with-quizzes/{id}")]
        public async Task<ActionResult<LessonWithQuizzesDto>> GetLessonWithQuizzes(int id)
        {
            var lesson = await _lessonService.GetLessonWithQuizzesAsync(id);
            if (lesson == null)
                return NotFound();

            return Ok(lesson);
        }

        [HttpPost("create-lessons")]
        public async Task<ActionResult<LessonDto>> CreateLesson(CreateLessonDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var lesson = await _lessonService.CreateLessonAsync(dto);
            return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lesson);
        }

        [HttpPut("update-lessons/{id}")]
        public async Task<ActionResult<LessonDto>> UpdateLesson(int id, UpdateLessonDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var lesson = await _lessonService.UpdateLessonAsync(id, dto);
            if (lesson == null)
                return NotFound();

            return Ok(lesson);
        }

        [HttpDelete("delete-lesson/{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var result = await _lessonService.DeleteLessonAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        #endregion Public Methods
    }
}