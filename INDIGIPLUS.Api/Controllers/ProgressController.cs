using INDIGIPLUS.Api.DTOs;
using INDIGIPLUS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace INDIGIPLUS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        [HttpGet]
        public async Task<ActionResult<UserProgressDto>> GetUserProgress()
        {
            var userId = GetUserId();
            if (userId == 0)
                return Unauthorized();

            var progress = await _progressService.GetUserProgressAsync(userId);
            return Ok(progress);
        }

        [HttpGet("achievements")]
        public async Task<ActionResult<List<AchievementDto>>> GetUserAchievements()
        {
            var userId = GetUserId();
            if (userId == 0)
                return Unauthorized();

            var achievements = await _progressService.GetUserAchievementsAsync(userId);
            return Ok(achievements);
        }

        [HttpPost("check-achievements")]
        public async Task<ActionResult> CheckAchievements()
        {
            var userId = GetUserId();
            if (userId == 0)
                return Unauthorized();

            await _progressService.CheckAndAwardAchievementsAsync(userId);
            return Ok();
        }
    }
}
