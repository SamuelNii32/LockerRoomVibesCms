using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LockerRoomVibesCms.Data;
using LockerRoomVibesCms.Interfaces;
using LockerRoomVibesCms.Models;
using Microsoft.AspNetCore.Hosting;

namespace LockerRoomVibesCms.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;   

        public PlaylistService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<PlaylistDto?> CreatePlaylistAsync(PlaylistDto playlistDto, IFormFile? coverImageFile, string wwwRootPath)
        {
            var team = await _context.Teams.FindAsync(playlistDto.TeamId);
            if (team == null)
                return null;

            string? coverImagePath = null;

            if (coverImageFile != null && coverImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(coverImageFile.FileName);
                var savePath = Path.Combine(wwwRootPath, "images", "playlists");

                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                var filePath = Path.Combine(savePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await coverImageFile.CopyToAsync(stream);
                }

                coverImagePath = $"/images/playlists/{fileName}";
            }
            else
            {
                // If no file uploaded, use any existing URL in DTO
                coverImagePath = playlistDto.CoverImageUrl;
            }

            var playlist = new Playlist
            {
                Title = playlistDto.Title,
                TeamId = playlistDto.TeamId,
                Description = playlistDto.Description,
                CoverImageUrl = coverImagePath,
                CreatedAt = DateTime.UtcNow
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return new PlaylistDto
            {
                Id = playlist.Id,
                Title = playlist.Title,
                TeamId = playlist.TeamId,
                TeamName = team.Name,
                Description = playlist.Description,
                CoverImageUrl = playlist.CoverImageUrl
            };
        }




        public async Task<bool> DeletePlaylistAsync(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
                return false;

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PlaylistDto> GetPlaylistAsync(int id)
        {
            var playlist = await _context.Playlists
                .Include(p => p.Team)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
                return null;

            return new PlaylistDto
            {
                Id = playlist.Id,
                Title = playlist.Title,
                TeamId = playlist.TeamId,
                TeamName = playlist.Team != null ? playlist.Team.Name : "Unknown",
                Description = playlist.Description,
                CoverImageUrl = playlist.CoverImageUrl
            };
        }

        public async Task<IEnumerable<PlaylistDto>> GetPlaylistsAsync()
        {
            return await _context.Playlists
                .Include(p => p.Team)
                .Select(p => new PlaylistDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    TeamId = p.TeamId,
                    TeamName = p.Team.Name,
                    Description = p.Description,
                    CoverImageUrl = p.CoverImageUrl,
                    TrackCount = p.PlaylistTracks.Count()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<PlaylistDto>> GetPlaylistsByTeamAsync(int teamId)
        {
            return await _context.Playlists
                .Where(p => p.TeamId == teamId)
                .Include(p => p.Team)
                .Select(p => new PlaylistDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    TeamId = p.TeamId,
                    TeamName = p.Team != null ? p.Team.Name : "Unknown",
                    Description = p.Description,
                    CoverImageUrl = p.CoverImageUrl,
                    TrackCount = p.PlaylistTracks.Count()

                })
                .ToListAsync();
        }

        public async Task<PlaylistDto?> UpdatePlaylistAsync(int id, PlaylistDto playlistDto)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
                return null;

            playlist.Title = playlistDto.Title;
            playlist.TeamId = playlistDto.TeamId;
            playlist.Description = playlistDto.Description;

            // Handle file upload
            if (playlistDto.CoverImageFile != null && playlistDto.CoverImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "playlists");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(playlistDto.CoverImageFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await playlistDto.CoverImageFile.CopyToAsync(stream);
                }

                playlist.CoverImageUrl = $"/images/playlists/{uniqueFileName}";
            }

            await _context.SaveChangesAsync();

            var team = await _context.Teams.FindAsync(playlist.TeamId);

            return new PlaylistDto
            {
                Id = playlist.Id,
                Title = playlist.Title,
                TeamId = playlist.TeamId,
                TeamName = team?.Name ?? "Unknown",
                Description = playlist.Description,
                CoverImageUrl = playlist.CoverImageUrl
            };
        }


        public async Task<List<PlaylistDto>> GetLatestPlaylistsAsync(int count = 2)
        {
            return await _context.Playlists
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .Select(p => new PlaylistDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    TeamId = p.TeamId,
                    TeamName = p.Team.Name,
                    CreatedAt = p.CreatedAt 
                })
                .ToListAsync();
        }

        public async Task<PlaylistDetailsDto> GetPlaylistDetailsAsync(int id)
        {
            var playlist = await _context.Playlists
                .Include(p => p.Team)
                .Include(p => p.PlaylistTracks)
                    .ThenInclude(pt => pt.Track)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
                return null;

            var tracks = playlist.PlaylistTracks
                .OrderBy(pt => pt.Position)
                .Select(pt => new PlaylistTrackDetailsDto
                {
                    Id = pt.Track.Id,
                    Title = pt.Track.Title,
                    Artist = pt.Track.Artist,
                    Mood = pt.Track.Mood,
                    DurationInSeconds = (int)pt.Track.Duration.TotalSeconds,
                    Position = pt.Position
                })
                .ToList();

            return new PlaylistDetailsDto
            {
                Id = playlist.Id,
                Title = playlist.Title,
                Description = playlist.Description,
                CoverImageUrl = playlist.CoverImageUrl,
                TeamId = playlist.TeamId,
                TeamName = playlist.Team?.Name,
                Tracks = tracks,
                TotalDurationInSeconds = tracks.Sum(t => t.DurationInSeconds)  // ✅ Here
            };
        }


    }
}