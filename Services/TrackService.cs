using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LockerRoomVibesCms.Data;
using LockerRoomVibesCms.Interfaces;
using LockerRoomVibesCms.Models;

namespace LockerRoomVibesCms.Services
{
    public class TrackService : ITrackService
    {
        private readonly ApplicationDbContext _context;

        public TrackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrackDto>> GetTracksAsync()
        {
            return await _context.Tracks
                .Include(t => t.PlaylistTracks)                 
                    .ThenInclude(pt => pt.Playlist)             
                .Select(t => new TrackDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Artist = t.Artist,
                    Mood = t.Mood,
                    AudioUrl = t.AudioUrl,
                    DurationInSeconds = (int)t.Duration.TotalSeconds,
                    PlaylistNames = t.PlaylistTracks
                                      .Select(pt => pt.Playlist.Title)
                                      .ToList()
                })
                .ToListAsync();
        }


        public async Task<TrackDto> GetTrackAsync(int id)
        {
            var track = await _context.Tracks.FindAsync(id);

            if (track == null)
                return null;

            return new TrackDto
            {
                Id = track.Id,
                Title = track.Title,
                Artist = track.Artist,
                Mood = track.Mood,
                AudioUrl = track.AudioUrl,
                DurationInSeconds = (int)track.Duration.TotalSeconds
            };
        }

        public async Task<TrackDetailsDto?> GetTrackDetailsAsync(int id)
        {
            var track = await _context.Tracks
                .Include(t => t.PlaylistTracks)
                    .ThenInclude(pt => pt.Playlist)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (track == null) return null;

            return new TrackDetailsDto
            {
                Id = track.Id,
                Title = track.Title,
                Artist = track.Artist,
                AudioUrl = track.AudioUrl,
                Playlists = track.PlaylistTracks.Select(pt => new TrackPlaylistDto
                {
                    PlaylistId = pt.PlaylistId,
                    PlaylistTitle = pt.Playlist.Title,
                    Position = pt.Position,
                    AudioUrl = pt.Playlist.CoverImageUrl ?? ""
                }).ToList()
            };
        }

        public async Task<TrackDto> CreateTrackAsync(TrackDto trackDto)
        {
            if (trackDto == null)
                throw new ArgumentNullException(nameof(trackDto));

            var track = new Track
            {
                Title = trackDto.Title,
                Artist = trackDto.Artist,
                Mood = trackDto.Mood,
                AudioUrl = trackDto.AudioUrl,
                Duration = TimeSpan.FromSeconds(trackDto.DurationInSeconds)
            };

            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();

            return new TrackDto
            {
                Id = track.Id,
                Title = track.Title,
                Artist = track.Artist,
                Mood = track.Mood,
                AudioUrl = track.AudioUrl,
                DurationInSeconds = (int)track.Duration.TotalSeconds
            };
        }


        public async Task<TrackDto> UpdateTrackAsync(int id, TrackDto trackDto)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
                return null;

            track.Title = trackDto.Title;
            track.Artist = trackDto.Artist;
            track.Mood = trackDto.Mood;
            track.AudioUrl = trackDto.AudioUrl;
            track.Duration = TimeSpan.FromSeconds(trackDto.DurationInSeconds);

            await _context.SaveChangesAsync();

            return new TrackDto
            {
                Id = track.Id,
                Title = track.Title,
                Artist = track.Artist,
                Mood = track.Mood,
                AudioUrl = track.AudioUrl,
                DurationInSeconds = trackDto.DurationInSeconds
            };
        }

        public async Task<bool> DeleteTrackAsync(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
                return false;

            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
