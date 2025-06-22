using Microsoft.EntityFrameworkCore;
using LockerRoomVibesCms.Data;
using LockerRoomVibesCms.Interfaces;
using LockerRoomVibesCms.Models;

namespace LockerRoomVibesCms.Services
{
    public class PlaylistTrackService : IPlaylistTrackService
    {
        private readonly ApplicationDbContext _context;

        public PlaylistTrackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddTrackToPlaylistAsync(PlaylistTrackDto dto)
        {
            var playlistExists = await _context.Playlists.AnyAsync(p => p.Id == dto.PlaylistId);
            var trackExists = await _context.Tracks.AnyAsync(t => t.Id == dto.TrackId);
            if (!playlistExists || !trackExists) return false;

            var existing = await _context.PlaylistTracks
                .FirstOrDefaultAsync(pt => pt.PlaylistId == dto.PlaylistId && pt.TrackId == dto.TrackId);

            if (existing != null) return false;

            // ✅ Shift tracks at or after the desired position up by 1
            var tracksToShift = await _context.PlaylistTracks
                .Where(pt => pt.PlaylistId == dto.PlaylistId && pt.Position >= dto.Position)
                .OrderByDescending(pt => pt.Position) // Start shifting from the end
                .ToListAsync();

            foreach (var pt in tracksToShift)
            {
                pt.Position += 1;
            }

            var playlistTrack = new PlaylistTrack
            {
                PlaylistId = dto.PlaylistId,
                TrackId = dto.TrackId,
                Position = dto.Position
            };

            _context.PlaylistTracks.Add(playlistTrack);
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<bool> UpdateTrackPositionAsync(PlaylistTrackDto dto)
        {
            var playlistTrack = await _context.PlaylistTracks
                .FirstOrDefaultAsync(pt => pt.PlaylistId == dto.PlaylistId && pt.TrackId == dto.TrackId);

            if (playlistTrack == null) return false;

            playlistTrack.Position = dto.Position;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveTrackFromPlaylistAsync(PlaylistTrackDto dto)
        {
            var playlistTrack = await _context.PlaylistTracks
                .FirstOrDefaultAsync(pt => pt.PlaylistId == dto.PlaylistId && pt.TrackId == dto.TrackId);

            if (playlistTrack == null) return false;

            int removedPosition = playlistTrack.Position;

            _context.PlaylistTracks.Remove(playlistTrack);

            // Shift down tracks after the removed one
            var tracksToUpdate = await _context.PlaylistTracks
                .Where(pt => pt.PlaylistId == dto.PlaylistId && pt.Position > removedPosition)
                .ToListAsync();

            foreach (var pt in tracksToUpdate)
            {
                pt.Position -= 1;
            }

            await _context.SaveChangesAsync();

            return true;
        }

    }

}
