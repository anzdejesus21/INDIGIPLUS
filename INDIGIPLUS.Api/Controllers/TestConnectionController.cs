using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INDIGIPLUS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TestConnectionController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Get()
        {
            return Ok("API is reachable.");
        }
    }
}
