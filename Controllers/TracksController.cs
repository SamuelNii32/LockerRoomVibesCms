using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LockerRoomVibesCms.Interfaces;
using LockerRoomVibesCms.Models;

namespace LockerRoomVibesCms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase

    {
        private readonly ITrackService _trackService;

        // Dependency injection of the track service interface
        public TracksController(ITrackService trackService)
        {
            _trackService = trackService;
        }


        /// <summary>
        /// Retrieves the list of all tracks.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// List of TrackDto objects
        /// </returns>
        /// <example>
        /// GET: api/Tracks
        /// </example>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackDto>>> GetTracks()
        {
            var tracks = await _trackService.GetTracksAsync();
            return Ok(tracks);
        }


        /// <summary>
        /// Retrieves a single track specified by its ID.
        /// </summary>
        /// <param name="id">The ID of the track</param>
        /// <returns>
        /// 200 OK with the TrackDto
        /// or
        /// 404 Not Found if the track does not exist
        /// </returns>
        /// <example>
        /// GET: api/Tracks/5
        /// </example>
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackDto>> GetTrack(int id)
        {
            var track = await _trackService.GetTrackAsync(id);
            if (track == null)
                return NotFound($"Track with ID {id} not found.");

            return Ok(track);
        }



        /// <summary>
        /// Creates a new track.
        /// </summary>
        /// <param name="trackDto">The TrackDto containing the track details</param>
        /// <returns>
        /// 201 Created with the created TrackDto
        /// or
        /// 400 Bad Request if creation fails
        /// </returns>
        /// <example>
        /// POST: api/Tracks
        /// Request Body: { TrackDto }
        /// </example>

        [HttpPost]
        public async Task<ActionResult<TrackDto>> CreateTrack(TrackDto trackDto)
        {
            var createdTrack = await _trackService.CreateTrackAsync(trackDto);
            if (createdTrack == null)
                return BadRequest("Failed to create track.");

            return CreatedAtAction(nameof(GetTrack), new { id = createdTrack.Id }, createdTrack);
        }

        /// <summary>
        /// Updates an existing track specified by ID.
        /// </summary>
        /// <param name="id">The ID of the track to update</param>
        /// <param name="trackDto">The updated TrackDto data</param>
        /// <returns>
        /// 200 OK with updated TrackDto
        /// or
        /// 404 Not Found if the track does not exist
        /// </returns>
        /// <example>
        /// PUT: api/Tracks/5
        /// Request Body: { TrackDto }
        /// </example>
        [HttpPut("{id}")]
        public async Task<ActionResult<TrackDto>> UpdateTrack(int id, TrackDto trackDto)
        {
            var updatedTrack = await _trackService.UpdateTrackAsync(id, trackDto);
            if (updatedTrack == null)
                return NotFound($"Track with ID {id} not found.");

            return Ok(updatedTrack);
        }



        /// <summary>
        /// Deletes a track specified by its ID.
        /// </summary>
        /// <param name="id">The ID of the track to delete</param>
        /// <returns>
        /// 204 No Content on successful deletion
        /// or
        /// 404 Not Found if the track does not exist
        /// </returns>
        /// <example>
        /// DELETE: api/Tracks/5
        /// </example>

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            var success = await _trackService.DeleteTrackAsync(id);
            if (!success)
                return NotFound($"Track with ID {id} not found.");

            return NoContent();
        }
    }
}
