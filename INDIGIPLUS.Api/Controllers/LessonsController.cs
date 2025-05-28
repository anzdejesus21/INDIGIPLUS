using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Entities;
using INDIGIPLUS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace INDIGIPLUS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<List<LessonDto>>> GetLessonsByCourse(int courseId)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var lessons = await _lessonService.GetLessonsByCourseAsync(courseId, userId);
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> GetLesson(int id)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var lesson = await _lessonService.GetLessonByIdAsync(id, userId);
            if (lesson == null) return NotFound();

            return Ok(lesson);
        }

        [HttpPost]
        public async Task<ActionResult<LessonDto>> CreateLesson([FromQuery] Lesson lesson)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdLesson = await _lessonService.CreateLessonAsync(lesson);
            return CreatedAtAction(nameof(GetLesson), new { id = createdLesson.Id }, createdLesson);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LessonDto>> UpdateLesson(int id, [FromBody] Lesson lesson)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedLesson = await _lessonService.UpdateLessonAsync(id, lesson);
            if (updatedLesson == null) return NotFound();

            return Ok(updatedLesson);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLesson(int id)
        {
            var result = await _lessonService.DeleteLessonAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/start")]
        public async Task<ActionResult> StartLesson(int id)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var result = await _lessonService.MarkLessonAsStartedAsync(id, userId);
            if (!result) return NotFound();

            return Ok();
        }

        [HttpPost("{id}/complete")]
        public async Task<ActionResult> CompleteLesson(int id)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var result = await _lessonService.MarkLessonAsCompletedAsync(id, userId);
            if (!result) return NotFound();

            return Ok();
        }
    }
}
