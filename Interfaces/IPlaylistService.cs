using System.Collections.Generic;
using System.Threading.Tasks;
using LockerRoomVibesCms.Models;

namespace LockerRoomVibesCms.Interfaces
{
    public interface IPlaylistService
    {
        Task<IEnumerable<PlaylistDto>> GetPlaylistsAsync();
        Task<IEnumerable<PlaylistDto>> GetPlaylistsByTeamAsync(int teamId);
        Task<PlaylistDto> GetPlaylistAsync(int id);
        Task<PlaylistDetailsDto> GetPlaylistDetailsAsync(int id);

        Task<PlaylistDto> CreatePlaylistAsync(PlaylistDto playlistDto);
        Task<PlaylistDto> UpdatePlaylistAsync(int id, PlaylistDto playlistDto);
        Task<bool> DeletePlaylistAsync(int id);
    }
}
