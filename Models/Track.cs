using System.ComponentModel.DataAnnotations;

namespace LockerRoomVibesCms.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Mood { get; set; }
        public string AudioUrl { get; set; }
        public TimeSpan Duration { get; set; }

        // Navigation: Many-to-many relationship with Playlist through PlaylistTrack
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }


    public class TrackDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Artist is required")]
        public string Artist { get; set; }

        [Required(ErrorMessage = "Mood is required")]
        public string Mood { get; set; }

        [Required(ErrorMessage = "Audio URL is required")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string AudioUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
        public int DurationInSeconds { get; set; }

        public List<string> PlaylistNames { get; set; } = new(); // Optional
    }


    public class TrackPlaylistDto
    {
        public int PlaylistId { get; set; }
        public string PlaylistTitle { get; set; }
        public int Position { get; set; }
        public string AudioUrl { get; set; }
    }

    public class TrackDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public string AudioUrl { get; set; }

    public List<TrackPlaylistDto> Playlists { get; set; }
}


}
