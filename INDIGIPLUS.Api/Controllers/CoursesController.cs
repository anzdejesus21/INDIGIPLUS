using CppLearningPlatform.Models;
using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace INDIGIPLUS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseDto>>> GetCourses()
        {
            var userId = GetUserId();
            if (userId == 0)
                return Unauthorized();

            var courses = await _courseService.GetCoursesAsync(userId);
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var userId = GetUserId();
            if (userId == 0)
                return Unauthorized();

            var course = await _courseService.GetCourseByIdAsync(id, userId);
            if (course == null)
                return NotFound();

            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] Course course)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCourse = await _courseService.CreateCourseAsync(course);
            return CreatedAtAction(nameof(GetCourse), new { id = createdCourse.Id }, createdCourse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CourseDto>> UpdateCourse(int id, [FromBody] Course course)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedCourse = await _courseService.UpdateCourseAsync(id, course);
            if (updatedCourse == null)
                return NotFound();

            return Ok(updatedCourse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
