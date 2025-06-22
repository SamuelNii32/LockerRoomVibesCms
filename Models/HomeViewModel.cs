using System.Collections.Generic;
using LockerRoomVibesCms.Models; 

namespace LockerRoomVibesCms.Models
{
    public class HomeViewModel
    {
        public List<TeamDto> LatestTeams { get; set; }
        public List<PlaylistDto> LatestPlaylists { get; set; }
    }
}
