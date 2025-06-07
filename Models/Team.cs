using Microsoft.EntityFrameworkCore;

namespace LockerRoomVibesCms.Models
{

    [Index(nameof(Name), IsUnique = true)]
    public class Team

    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? LogoUrl { get; set; }


        //Navigation: One team can have many playlists
        public ICollection<Playlist> Playlists { get; set; }
    }

    public class TeamDto { 
        public int id { get; set; }
        public string Name { get; set; }
        public string? LogoUrl { get; set; }
    }
}
