﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LockerRoomVibesCms.Models
{
    public class Playlist
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public string? CoverImageUrl { get; set; }

        [Required]
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }

    // Basic DTO for listing or creating playlists
    public class PlaylistDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int TeamId { get; set; }
        public string ? TeamName { get; set; }

        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
    }

    // Detailed DTO for showing more info (used in Playlist details)
    public class PlaylistDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }

        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public List<TrackDto> Tracks { get; set; }
    }
}
