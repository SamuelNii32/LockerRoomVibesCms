using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LockerRoomVibesCms.Data;
using LockerRoomVibesCms.Interfaces;
using LockerRoomVibesCms.Models;

namespace LockerRoomVibesCms.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ApplicationDbContext _context;

        public PlaylistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PlaylistDto> CreatePlaylistAsync(PlaylistDto playlistDto)
        {
            var team = await _context.Teams.FindAsync(playlistDto.TeamId);
            if (team == null)
                return null;

            var playlist = new Playlist
            {
                Title = playlistDto.Title,
                TeamId = playlistDto.TeamId,
                Description = playlistDto.Description,
                CoverImageUrl = playlistDto.CoverImageUrl
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            // Return a new DTO with full data
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
                    CoverImageUrl = p.CoverImageUrl
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
                    CoverImageUrl = p.CoverImageUrl

                })
                .ToListAsync();
        }

        public async Task<PlaylistDto> UpdatePlaylistAsync(int id, PlaylistDto playlistDto)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
                return null;

            playlist.Title = playlistDto.Title;
            playlist.TeamId = playlistDto.TeamId;
            playlist.Description = playlistDto.Description;
            playlist.CoverImageUrl = playlistDto.CoverImageUrl;

            await _context.SaveChangesAsync();

            // Fetch team name for DTO  
            var team = await _context.Teams.FindAsync(playlist.TeamId);

            return new PlaylistDto
            {
                Id = playlist.Id,
                Title = playlist.Title,
                TeamId = playlist.TeamId,
                TeamName = team != null ? team.Name : "Unknown",
                Description = playlist.Description,
                CoverImageUrl = playlist.CoverImageUrl
            };
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

            return new PlaylistDetailsDto
            {
                Id = playlist.Id,
                Title = playlist.Title,
                Description = playlist.Description,
                CoverImageUrl = playlist.CoverImageUrl,
                TeamId = playlist.TeamId,
                TeamName = playlist.Team?.Name,
                Tracks = playlist.PlaylistTracks
                    .OrderBy(pt => pt.Position)
                    .Select(pt => new TrackDto
                    {
                        Id = pt.Track.Id,
                        Title = pt.Track.Title,
                        Artist = pt.Track.Artist,
                        Mood = pt.Track.Mood,
                        DurationInSeconds = (int)pt.Track.Duration.TotalSeconds
                    })
                    .ToList()
            };
        }

    }
}