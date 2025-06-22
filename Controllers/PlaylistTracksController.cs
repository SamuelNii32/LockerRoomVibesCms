using Microsoft.AspNetCore.Mvc;
using LockerRoomVibesCms.Interfaces;
using System.Threading.Tasks;
using LockerRoomVibesCms.Models;

namespace LockerRoomVibesCms.Controllers
{
    public class PlaylistTracksController : Controller
    {
        private readonly IPlaylistTrackService _playlistTrackService;
        private readonly IPlaylistService _playlistService;

        public PlaylistTracksController(IPlaylistTrackService playlistTrackService, IPlaylistService playlistService)
        {
            _playlistTrackService = playlistTrackService;
            _playlistService = playlistService;
        }

        // GET: /PlaylistTracks
        public async Task<IActionResult> Index(int? playlistId)
        {
            var playlists = await _playlistService.GetPlaylistsAsync();
            ViewData["Playlists"] = playlists;

            if (playlistId.HasValue)
            {
                ViewData["SelectedPlaylistId"] = playlistId.Value;
            }

            return View();
        }

        // POST: /PlaylistTrack/Add
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PlaylistTrackDto dto)
        {
            Console.WriteLine($"Received Add: PlaylistId={dto.PlaylistId}, TrackId={dto.TrackId}, Position={dto.Position}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state invalid.");
                return BadRequest("Invalid data.");
            }

            var success = await _playlistTrackService.AddTrackToPlaylistAsync(dto);
            if (!success)
            {
                Console.WriteLine("AddTrackToPlaylistAsync returned false.");
                return BadRequest("Could not add track to playlist.");
            }

            return Ok(new { message = "Track added successfully." });
        }



        // POST: /PlaylistTrack/Update
        [HttpPost]
        public async Task<IActionResult> Update(PlaylistTrackDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var success = await _playlistTrackService.UpdateTrackPositionAsync(dto);
            if (!success)
                return BadRequest("Could not update track position.");

            return Ok();
        }

        // POST: /PlaylistTrack/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove([FromBody] PlaylistTrackDto dto)
        {
            if (dto == null || dto.PlaylistId == 0 || dto.TrackId == 0)
                return BadRequest("Invalid request");

            var success = await _playlistTrackService.RemoveTrackFromPlaylistAsync(dto);

            if (!success)
                return BadRequest("Could not remove track from playlist.");

            return Ok("Track removed successfully");
        }

    }
}
