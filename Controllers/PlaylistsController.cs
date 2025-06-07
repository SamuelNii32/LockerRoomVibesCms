using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LockerRoomVibesCms.Data;
using LockerRoomVibesCms.Models;
using LockerRoomVibesCms.Interfaces;

namespace LockerRoomVibesCms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistsController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetPlaylists()
        {
            var playlists = await _playlistService.GetPlaylistsAsync();
            return Ok(playlists);
        }

        [HttpGet("/api/teams/{teamId}/playlists")]
        public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetPlaylistsByTeam(int teamId)
        {
            var playlists = await _playlistService.GetPlaylistsByTeamAsync(teamId);

            if (playlists == null || !playlists.Any())
            {
                return NotFound($"No playlists found for Team ID {teamId}");
            }

            return Ok(playlists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistDto>> GetPlaylist(int id)
        {
            var playlist = await _playlistService.GetPlaylistAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return Ok(playlist);
        }

        [HttpPost]
        public async Task<ActionResult<PlaylistDto>> CreatePlaylist(PlaylistDto playlistDto)
        {
            var createdPlaylist = await _playlistService.CreatePlaylistAsync(playlistDto);

            if (createdPlaylist == null)
            {
                // Return 400 Bad Request
                return BadRequest($"Team with ID {playlistDto.TeamId} does not exist.");
            }

            return CreatedAtAction(nameof(GetPlaylist), new { id = createdPlaylist.Id }, createdPlaylist);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlaylist(int id, PlaylistDto playlistDto)
        {
            var updatedPlaylist = await _playlistService.UpdatePlaylistAsync(id, playlistDto);

            if (updatedPlaylist == null)
            {
                return NotFound();
            }

            return Ok(updatedPlaylist);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var deleted = await _playlistService.DeletePlaylistAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent(); // 204 No Content is standard for successful DELETE
        }


        [HttpGet("{id}/details")]
        public async Task<ActionResult<PlaylistDetailsDto>> GetPlaylistDetails(int id)
        {
            var details = await _playlistService.GetPlaylistDetailsAsync(id);
            if (details == null)
                return NotFound($"No playlist found with ID {id}");

            return Ok(details);
        }




    }
}
