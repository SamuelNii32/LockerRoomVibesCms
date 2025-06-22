using Microsoft.AspNetCore.Mvc;
using LockerRoomVibesCms.Interfaces;
using LockerRoomVibesCms.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace LockerRoomVibesCms.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly ITeamService _teamService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlaylistsController(IPlaylistService playlistService, ITeamService teamService, IWebHostEnvironment webHostEnvironment)
        {
            _playlistService = playlistService;
            _teamService = teamService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /Playlist
        public async Task<IActionResult> Index()
        {
            var playlists = await _playlistService.GetPlaylistsAsync();
            var teams = await _teamService.GetTeamsAsync();
            ViewData["Teams"] = teams;  
                                        
            return View(playlists); 
        }


        [HttpGet("/Playlists/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var playlists = await _playlistService.GetPlaylistsAsync();
            return Json(playlists);
        }

        // GET: /Playlist/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var playlistDetails = await _playlistService.GetPlaylistDetailsAsync(id);
            if (playlistDetails == null) return NotFound();
            return View(playlistDetails); // Views/Playlist/Details.cshtml
        }

        [HttpGet("/Playlists/GetDetails/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var playlistDetails = await _playlistService.GetPlaylistDetailsAsync(id);
            if (playlistDetails == null) return NotFound();
            return Json(playlistDetails);
        }


        // GET: /Playlist/Create
        public async Task<IActionResult> Create(int? teamId)
        {
            var teams = await _teamService.GetTeamsAsync();
            ViewData["Teams"] = teams;

            var playlistDto = new PlaylistDto();

            if (teamId.HasValue)
            {
                playlistDto.TeamId = teamId.Value;
            }

            return View(playlistDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlaylistDto playlistDto, IFormFile? coverImageFile)
        {
            if (!ModelState.IsValid)
            {
                var teams = await _teamService.GetTeamsAsync();
                ViewData["Teams"] = teams;
                return View(playlistDto);
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            var result = await _playlistService.CreatePlaylistAsync(playlistDto, coverImageFile, wwwRootPath);

            if (result == null)
            {
                ModelState.AddModelError("", "Failed to create playlist. Ensure the Team ID exists.");

                var teams = await _teamService.GetTeamsAsync();
                ViewData["Teams"] = teams;

                return View(playlistDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> CreateAjax(PlaylistDto playlistDto, IFormFile? coverImageFile)
{
    if (!ModelState.IsValid)
    {
        // Return validation errors as JSON
        var errors = ModelState.Values
                       .SelectMany(v => v.Errors)
                       .Select(e => e.ErrorMessage)
                       .ToList();

        return BadRequest(new { success = false, errors });
    }

    string wwwRootPath = _webHostEnvironment.WebRootPath;

    var createdPlaylist = await _playlistService.CreatePlaylistAsync(playlistDto, coverImageFile, wwwRootPath);

    if (createdPlaylist == null)
    {
        return BadRequest(new { success = false, errors = new[] { "Failed to create playlist." } });
    }

    // Return success and new playlist data (adjust as needed)
    return Json(new
    {
        success = true,
        playlist = new
        {
            id = createdPlaylist.Id,
            title = createdPlaylist.Title,
            description = createdPlaylist.Description,
            coverImageUrl = createdPlaylist.CoverImageUrl,
            teamId = createdPlaylist.TeamId,
            trackCount = 0
        }
    });
}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] PlaylistDto playlistDto)
        {
            if (id != playlistDto.Id)
                return BadRequest(new { success = false, message = "ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data." });

            var updated = await _playlistService.UpdatePlaylistAsync(id, playlistDto);
            if (updated == null)
                return NotFound(new { success = false, message = "Playlist not found." });

            return Json(new
            {
                success = true,
                id = updated.Id,
                title = updated.Title,
                teamId = updated.TeamId,
                teamName = updated.TeamName,
                description = updated.Description,
                coverImageUrl = updated.CoverImageUrl,
                trackCount = updated.TrackCount
            });
        }


        // POST: /Playlist/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _playlistService.DeletePlaylistAsync(id);
            if (!deleted)
                return NotFound(new { success = false, message = "Playlist not found." });

            return Json(new { success = true });
        }


        public async Task<IActionResult> EditModal(int id)
        {
            var playlist = await _playlistService.GetPlaylistAsync(id);
            if (playlist == null) return NotFound();
            return PartialView("_EditModal", playlist); 
        }

        
        public async Task<IActionResult> DeleteModal(int id)
        {
            var playlist = await _playlistService.GetPlaylistAsync(id);
            if (playlist == null) return NotFound();
            return PartialView("_DeleteModal", playlist); 
        }

    }
}
