using LockerRoomVibesCms.Models;

namespace LockerRoomVibesCms.Interfaces
{
    public interface IPlaylistTrackService
    {
        Task<bool> AddTrackToPlaylistAsync(PlaylistTrackDto dto);
        Task<bool> UpdateTrackPositionAsync(PlaylistTrackDto dto);
        Task<bool> RemoveTrackFromPlaylistAsync(PlaylistTrackDto dto);
    }


}
