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
                .Select(t => new TrackDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Artist = t.Artist,
                    Mood = t.Mood,
                    DurationInSeconds = (int)t.Duration.TotalSeconds
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
                DurationInSeconds = (int)track.Duration.TotalSeconds
            };
        }

        public async Task<TrackDto> CreateTrackAsync(TrackDto trackDto)
        {
            var track = new Track
            {
                Title = trackDto.Title,
                Artist = trackDto.Artist,
                Mood = trackDto.Mood,
                Duration = TimeSpan.FromSeconds(trackDto.DurationInSeconds)
            };

            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();

            trackDto.Id = track.Id;
            return trackDto;
        }

        public async Task<TrackDto> UpdateTrackAsync(int id, TrackDto trackDto)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
                return null;

            track.Title = trackDto.Title;
            track.Artist = trackDto.Artist;
            track.Mood = trackDto.Mood;
            track.Duration = TimeSpan.FromSeconds(trackDto.DurationInSeconds);

            await _context.SaveChangesAsync();

            return new TrackDto
            {
                Id = track.Id,
                Title = track.Title,
                Artist = track.Artist,
                Mood = track.Mood,
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
