using System.Collections.Generic;
using System.Threading.Tasks;
using LockerRoomVibesCms.Models;

namespace LockerRoomVibesCms.Interfaces
{
    public interface ITrackService
    {
        Task<IEnumerable<TrackDto>> GetTracksAsync();
        Task<TrackDto> GetTrackAsync(int id);
        Task<TrackDto> CreateTrackAsync(TrackDto trackDto);
        Task<TrackDto> UpdateTrackAsync(int id, TrackDto trackDto);
        Task<bool> DeleteTrackAsync(int id);
    }
}
