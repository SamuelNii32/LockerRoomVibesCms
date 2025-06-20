﻿namespace LockerRoomVibesCms.Models
{
    public class PlaylistTrack
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public int TrackId { get; set; }
        public Track Track { get; set; }

        // New field: track order within a playlist
        public int Position { get; set; }
    }

    public class PlaylistTrackDto
    {
        public int PlaylistId { get; set; }
        public int TrackId { get; set; }
        public int Position { get; set; }  
    }

}
