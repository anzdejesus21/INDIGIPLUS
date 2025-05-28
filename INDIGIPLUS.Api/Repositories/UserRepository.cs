using INDIGIPLUS.Api.Data;
using INDIGIPLUS.Api.Entities;
using INDIGIPLUS.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INDIGIPLUS.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion Fields

        #region Public Constructors

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion Public Methods
    }
}