using INDIGIPLUS.Api.DTOs;

namespace INDIGIPLUS.Api.Services.Interfaces
{
    public interface IProgressService
    {
        Task<UserProgressDto> GetUserProgressAsync(int userId);
        Task<List<AchievementDto>> GetUserAchievementsAsync(int userId);
        Task CheckAndAwardAchievementsAsync(int userId);
    }
}
