using Microsoft.EntityFrameworkCore;

namespace LockerRoomVibesCms.Models
{

    [Index(nameof(Name), IsUnique = true)]
    public class Team

    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? LogoUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        //Navigation: One team can have many playlists
        public ICollection<Playlist> Playlists { get; set; }
    }

    public class TeamDto { 
        public int id { get; set; }
        public string Name { get; set; }
        public string? LogoUrl { get; set; }
        public int PlaylistCount { get; set; }
        public IFormFile? LogoFile { get; set; }
    }

    public class TeamDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? LogoUrl { get; set; }
        public int PlaylistCount { get; set; }
        public List<PlaylistDto> Playlists { get; set; }
    }

}

