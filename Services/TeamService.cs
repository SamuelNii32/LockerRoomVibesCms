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
        private readonly IPlaylistService _playlistService;

        public TeamService(ApplicationDbContext context, IPlaylistService playlistService)
        {
            _context = context;
            _playlistService = playlistService;

        }

        public async Task<IEnumerable<TeamDto>> GetTeamsAsync()
        {
            return await _context.Teams
                .Select(team => new TeamDto
                {
                    id = team.Id,
                    Name = team.Name,
                    LogoUrl = team.LogoUrl,
                    PlaylistCount = team.Playlists.Count()
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





        public async Task<TeamDto> CreateTeamAsync(TeamDto teamDto, IFormFile logoFile, string wwwRootPath)
        {
            // Check if a team with the same name already exists (case-insensitive)
            var existingTeam = await _context.Teams
                .FirstOrDefaultAsync(t => t.Name.ToLower() == teamDto.Name.ToLower());

            if (existingTeam != null)
                throw new InvalidOperationException("A team with the same name already exists.");

            string logoPath = null;

            if (logoFile != null && logoFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
                var savePath = Path.Combine(wwwRootPath, "images", "teams");

                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                var filePath = Path.Combine(savePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await logoFile.CopyToAsync(stream);
                }

                // Public-facing path used in <img src="">
                logoPath = $"/images/teams/{fileName}";
            }

            var team = new Team
            {
                Name = teamDto.Name,
                LogoUrl = logoPath
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            teamDto.id = team.Id;
            teamDto.LogoUrl = team.LogoUrl;

            return teamDto;
        }

        public async Task<TeamDetailsDto> GetTeamDetailsAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return null;
            }

            var playlists = await _playlistService.GetPlaylistsByTeamAsync(id);

            return new TeamDetailsDto
            {
                Id = team.Id,
                Name = team.Name,
                LogoUrl = team.LogoUrl,
                PlaylistCount = playlists.Count(),
                Playlists = playlists.ToList()
            };
        }



        public async Task<TeamDto> UpdateTeamAsync(int id, TeamDto teamDto, IFormFile logoFile, string wwwRootPath)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return null;
            }

            // Update name
            team.Name = teamDto.Name;

            // Handle new logo file if provided
            if (logoFile != null && logoFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
                var savePath = Path.Combine(wwwRootPath, "images", "teams");

                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                var filePath = Path.Combine(savePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await logoFile.CopyToAsync(stream);
                }

                // Update LogoUrl to new public path
                team.LogoUrl = $"/images/teams/{fileName}";
            }

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


        public async Task<List<TeamDto>> GetLatestTeamsAsync(int count = 2)
        {
            return await _context.Teams
                .OrderByDescending(t => t.CreatedAt)
                .Take(count)
                .Select(t => new TeamDto
                {
                    id = t.Id,
                    Name = t.Name,
                    LogoUrl = t.LogoUrl
                })
                .ToListAsync();
        }

    }

}
