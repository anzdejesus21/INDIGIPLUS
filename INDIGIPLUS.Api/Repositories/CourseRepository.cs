using CppLearningPlatform.Models;
using INDIGIPLUS.Api.Data;
using INDIGIPLUS.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INDIGIPLUS.Api.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion Fields

        #region Public Constructors

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<List<Course>> GetActiveCoursesAsync()
        {
            return await _context.Courses
                .Where(c => c.IsActive)
                .Include(c => c.Lessons)
                .ThenInclude(l => l.UserProgresses)
                .OrderBy(c => c.Order)
                .ToListAsync();
        }

        public async Task<Course?> GetActiveCourseByIdAsync(int courseId)
        {
            return await _context.Courses
                .Where(c => c.Id == courseId && c.IsActive)
                .Include(c => c.Lessons)
                .ThenInclude(l => l.UserProgresses)
                .FirstOrDefaultAsync();
        }

        public async Task AddCourseAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
        }

        public async Task<Course?> FindCourseByIdAsync(int courseId)
        {
            return await _context.Courses.FindAsync(courseId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion Public Methods
    }
}