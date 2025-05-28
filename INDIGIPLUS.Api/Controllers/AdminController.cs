using INDIGIPLUS.Api.Common.Enums;
using INDIGIPLUS.Api.Common.Response;
using INDIGIPLUS.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INDIGIPLUS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("users")]
        public ActionResult<IEnumerable<UserInfo>> GetAllUsers()
        {
            var users = _context.Users
                .Where(u => u.IsActive)
                .Select(u => new UserInfo
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Role = u.Role
                })
                .ToList();

            return Ok(users);
        }

        [HttpGet("dashboard")]
        public ActionResult<object> GetDashboardData()
        {
            var totalStudents = _context.Users.Count(u => u.Role == UserRole.Student && u.IsActive);
            var totalAdmins = _context.Users.Count(u => u.Role == UserRole.Admin && u.IsActive);

            return Ok(new
            {
                totalStudents,
                totalAdmins,
                totalUsers = totalStudents + totalAdmins
            });
        }
    }
}
