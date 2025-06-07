using Microsoft.AspNetCore.Mvc;
using LockerRoomVibesCms.Models;
using LockerRoomVibesCms.Interfaces;

namespace LockerRoomVibesCms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistTracksController : ControllerBase
    {
        private readonly IPlaylistTrackService _playlistTrackService;


        // Inject the PlaylistTrack service
        public PlaylistTracksController(IPlaylistTrackService playlistTrackService)
        {
            _playlistTrackService = playlistTrackService;
        }


        /// <summary>
        /// Adds a track to a playlist.
        /// </summary>
        /// <param name="dto">PlaylistTrackDto containing playlist and track IDs</param>
        /// <returns>200 OK if successful, 400 Bad Request otherwise</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddTrackToPlaylist([FromBody] PlaylistTrackDto dto)
        {
            var success = await _playlistTrackService.AddTrackToPlaylistAsync(dto);
            if (!success) return BadRequest("Could not add track to playlist.");
            return Ok(new { message = "Track added successfully." });
        }



        /// <summary>
        /// Updates the position of a track within a playlist.
        /// </summary>
        /// <param name="dto">PlaylistTrackDto containing playlist, track IDs and new position</param>
        /// <returns>200 OK if successful, 400 Bad Request otherwise</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTrackPosition([FromBody] PlaylistTrackDto dto)
        {
            var success = await _playlistTrackService.UpdateTrackPositionAsync(dto);
            if (!success) return BadRequest("Could not update track position.");
            return Ok();
        }


        /// <summary>
        /// Removes a track from a playlist.
        /// </summary>
        /// <param name="dto">PlaylistTrackDto containing playlist and track IDs</param>
        /// <returns>200 OK if successful, 400 Bad Request otherwise</returns>

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveTrackFromPlaylist([FromBody] PlaylistTrackDto dto)
        {
            var success = await _playlistTrackService.RemoveTrackFromPlaylistAsync(dto);
            if (!success) return BadRequest("Could not remove track from playlist.");
            return Ok();
        }



    }

}
