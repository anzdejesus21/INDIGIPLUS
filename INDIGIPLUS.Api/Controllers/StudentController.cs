using Microsoft.AspNetCore.Mvc;

namespace INDIGIPLUS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        [HttpGet("profile")]
        public ActionResult GetProfile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var name = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;
            var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            return Ok(new
            {
                userId,
                name,
                email,
                message = "Welcome to your student dashboard!"
            });
        }

        [HttpGet("courses")]
        public ActionResult GetCourses()
        {
            // Mock data for demonstration
            var courses = new[]
            {
                new { id = 1, name = "Mathematics", instructor = "Dr. Smith", credits = 3 },
                new { id = 2, name = "Physics", instructor = "Dr. Johnson", credits = 4 },
                new { id = 3, name = "Chemistry", instructor = "Dr. Brown", credits = 3 }
            };

            return Ok(courses);
        }
    }
}
