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

        public PlaylistTracksController(IPlaylistTrackService playlistTrackService)
        {
            _playlistTrackService = playlistTrackService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTrackToPlaylist([FromBody] PlaylistTrackDto dto)
        {
            var success = await _playlistTrackService.AddTrackToPlaylistAsync(dto);
            if (!success) return BadRequest("Could not add track to playlist.");
            return Ok();
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateTrackPosition([FromBody] PlaylistTrackDto dto)
        {
            var success = await _playlistTrackService.UpdateTrackPositionAsync(dto);
            if (!success) return BadRequest("Could not update track position.");
            return Ok();
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveTrackFromPlaylist([FromBody] PlaylistTrackDto dto)
        {
            var success = await _playlistTrackService.RemoveTrackFromPlaylistAsync(dto);
            if (!success) return BadRequest("Could not remove track from playlist.");
            return Ok();
        }



    }

}
