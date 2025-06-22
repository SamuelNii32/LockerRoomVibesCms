using System.Threading.Tasks;
using LockerRoomVibesCms.Models; // For TeamDto

namespace LockerRoomVibesCms.Interfaces


{
    public interface ITeamService
    {
        Task<IEnumerable<TeamDto>> GetTeamsAsync();
        Task<TeamDto> GetTeamAsync(int id);
        Task<TeamDetailsDto> GetTeamDetailsAsync(int id);
        Task<TeamDto> CreateTeamAsync(TeamDto teamDto, IFormFile logoFile, string wwwRootPath);

        Task<List<TeamDto>> GetLatestTeamsAsync(int count = 2);

        Task<TeamDto> UpdateTeamAsync(int id, TeamDto teamDto, IFormFile logoFile, string wwwRootPath);
        Task<bool> DeleteTeamAsync(int id);

    }
}
