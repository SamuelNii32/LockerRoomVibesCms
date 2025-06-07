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

        // Dependency injection of playlist service interface
        public PlaylistsController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }


        /// <summary>
        /// Retrieves all playlists.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// List of PlaylistDto objects
        /// </returns>
        /// <example>
        /// GET: api/Playlists
        /// </example>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetPlaylists()
        {
            var playlists = await _playlistService.GetPlaylistsAsync();
            return Ok(playlists);
        }


        /// <summary>
        /// Retrieves playlists belonging to a specific team.
        /// </summary>
        /// <param name="teamId">The ID of the team</param>
        /// <returns>
        /// 200 OK with list of PlaylistDto
        /// or
        /// 404 Not Found if no playlists exist for the team
        /// </returns>
        /// <example>
        /// GET: api/teams/3/playlists
        /// </example>
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

        /// <summary>
        /// Retrieves a single playlist by its ID.
        /// </summary>
        /// <param name="id">The playlist ID</param>
        /// <returns>
        /// 200 OK with PlaylistDto
        /// or
        /// 404 Not Found if playlist does not exist
        /// </returns>
        /// <example>
        /// GET: api/Playlists/5
        /// </example>
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


        /// <summary>
        /// Creates a new playlist.
        /// </summary>
        /// <param name="playlistDto">Playlist data to create</param>
        /// <returns>
        /// 201 Created with PlaylistDto
        /// or
        /// 400 Bad Request if related TeamId does not exist
        /// </returns>
        /// <example>
        /// POST: api/Playlists
        /// Request Body: { PlaylistDto }
        /// </example>
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


        /// <summary>
        /// Updates an existing playlist by ID.
        /// </summary>
        /// <param name="id">The ID of the playlist to update</param>
        /// <param name="playlistDto">The updated playlist data</param>
        /// <returns>
        /// 200 OK with updated PlaylistDto
        /// or
        /// 404 Not Found if playlist does not exist
        /// </returns>
        /// <example>
        /// PUT: api/Playlists/5
        /// Request Body: { PlaylistDto }
        /// </example>

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

        /// <summary>
        /// Deletes a playlist specified by ID.
        /// </summary>
        /// <param name="id">The ID of the playlist to delete</param>
        /// <returns>
        /// 204 No Content on successful deletion
        /// or
        /// 404 Not Found if playlist does not exist
        /// </returns>
        /// <example>
        /// DELETE: api/Playlists/5
        /// </example>
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


        /// <summary>
        /// Retrieves detailed info for a playlist, including associated tracks.
        /// </summary>
        /// <param name="id">The playlist ID</param>
        /// <returns>
        /// 200 OK with PlaylistDetailsDto
        /// or
        /// 404 Not Found if playlist does not exist
        /// </returns>
        /// <example>
        /// GET: api/Playlists/5/details
        /// </example>

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
