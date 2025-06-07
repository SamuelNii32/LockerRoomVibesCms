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

        public TracksController(ITrackService trackService)
        {
            _trackService = trackService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackDto>>> GetTracks()
        {
            var tracks = await _trackService.GetTracksAsync();
            return Ok(tracks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrackDto>> GetTrack(int id)
        {
            var track = await _trackService.GetTrackAsync(id);
            if (track == null)
                return NotFound($"Track with ID {id} not found.");

            return Ok(track);
        }

        [HttpPost]
        public async Task<ActionResult<TrackDto>> CreateTrack(TrackDto trackDto)
        {
            var createdTrack = await _trackService.CreateTrackAsync(trackDto);
            if (createdTrack == null)
                return BadRequest("Failed to create track.");

            return CreatedAtAction(nameof(GetTrack), new { id = createdTrack.Id }, createdTrack);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TrackDto>> UpdateTrack(int id, TrackDto trackDto)
        {
            var updatedTrack = await _trackService.UpdateTrackAsync(id, trackDto);
            if (updatedTrack == null)
                return NotFound($"Track with ID {id} not found.");

            return Ok(updatedTrack);
        }

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
