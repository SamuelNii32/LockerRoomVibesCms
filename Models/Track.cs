namespace LockerRoomVibesCms.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Mood { get; set; }
        public TimeSpan Duration { get; set; }

        // Navigation: Many-to-many relationship with Playlist through PlaylistTrack
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }


    public class TrackDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Mood { get; set; }
        public int DurationInSeconds { get; set; }
    }



}
