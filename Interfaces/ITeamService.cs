﻿using System.Threading.Tasks;
using LockerRoomVibesCms.Models; // For TeamDto

namespace LockerRoomVibesCms.Interfaces


{
    public interface ITeamService
    {
        Task<IEnumerable<TeamDto>> GetTeamsAsync();
        Task<TeamDto> GetTeamAsync(int id);
        Task<TeamDto> CreateTeamAsync(TeamDto teamDto);
        Task<TeamDto> UpdateTeamAsync(int id, TeamDto teamDto);
        Task<bool> DeleteTeamAsync(int id);

    }
}
