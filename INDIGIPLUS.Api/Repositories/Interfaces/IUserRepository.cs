using INDIGIPLUS.Api.Entities;

namespace INDIGIPLUS.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        #region Public Methods

        Task<User?> GetByEmailAsync(string email);

        Task AddAsync(User user);

        Task SaveChangesAsync();

        #endregion Public Methods
    }
}