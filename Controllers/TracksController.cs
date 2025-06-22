using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LockerRoomVibesCms.Interfaces;
using LockerRoomVibesCms.Models;

namespace LockerRoomVibesCms.Controllers
{
    public class TracksController : Controller
    {
        private readonly ITrackService _trackService;

        public TracksController(ITrackService trackService)
        {
            _trackService = trackService;
        }

        // GET: Tracks
        public async Task<IActionResult> Index()
        {
            var tracks = await _trackService.GetTracksAsync();
            return View(tracks); // View will be Views/Tracks/Index.cshtml
        }


        [HttpGet("/Tracks/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var tracks = await _trackService.GetTracksAsync(); 
            return Json(tracks);
        }


        public async Task<IActionResult> Details(int id)
        {
            var track = await _trackService.GetTrackDetailsAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }


        // GET: Tracks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tracks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrackDto trackDto)
        {
            if (!ModelState.IsValid)
                return View(trackDto);

            await _trackService.CreateTrackAsync(trackDto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tracks/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var track = await _trackService.GetTrackAsync(id);
            if (track == null)
                return NotFound();

            return View(track);
        }

        // POST: Tracks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrackDto trackDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data." });

            await _trackService.UpdateTrackAsync(id, trackDto);
            return Json(new
            {
                success = true,
                title = trackDto.Title,
                artist = trackDto.Artist,
                mood = trackDto.Mood,
                audioUrl = trackDto.AudioUrl,
                durationInSeconds = trackDto.DurationInSeconds
            });
        }

        
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _trackService.DeleteTrackAsync(id);
            return Json(new { success = true });
        }



    }
}
