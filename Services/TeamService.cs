using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;                 // For ToListAsync()
using LockerRoomVibesCms.Models;                     // For TeamDto
using LockerRoomVibesCms.Interfaces;                 // For ITeamService
using LockerRoomVibesCms.Data;                       // For ApplicationDbContext (your DbContext namespace)

namespace LockerRoomVibesCms.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeamDto>> GetTeamsAsync()
        {
            return await _context.Teams
                .Select(team => new TeamDto
                {
                    id = team.Id,
                    Name = team.Name,
                    LogoUrl = team.LogoUrl
                })
                .ToListAsync();
        }

        public async Task<TeamDto> GetTeamAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return null;
            }
            return new TeamDto
            {
                id = team.Id,
                Name = team.Name,
                LogoUrl = team.LogoUrl
            };
        }

        public async Task<TeamDto> CreateTeamAsync(TeamDto teamDto)
        {
            // Check if a team with the same name already exists (case-insensitive)
            var existingTeam = await _context.Teams
                .FirstOrDefaultAsync(t => t.Name.ToLower() == teamDto.Name.ToLower());

            if (existingTeam != null)
            {
                // Throw an exception or handle as you prefer
                throw new InvalidOperationException("A team with the same name already exists.");
            }

            var team = new Team
            {
                Name = teamDto.Name,
                LogoUrl = teamDto.LogoUrl
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            teamDto.id = team.Id;
            return teamDto;
        }


        public async Task<TeamDto> UpdateTeamAsync(int id, TeamDto teamDto)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return null;
            }
            team.Name = teamDto.Name;
            team.LogoUrl = teamDto.LogoUrl;
            await _context.SaveChangesAsync();
            return new TeamDto
            {
                id = team.Id,
                Name = team.Name,
                LogoUrl = team.LogoUrl
            };
        }

        public async Task<bool> DeleteTeamAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return false;
            }
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return true;
        }
    }
       
}
