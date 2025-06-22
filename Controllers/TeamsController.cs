using Microsoft.AspNetCore.Mvc;
using LockerRoomVibesCms.Interfaces;
using LockerRoomVibesCms.Models;
using Microsoft.AspNetCore.Hosting;


namespace LockerRoomVibesCms.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamsController(ITeamService teamService, IWebHostEnvironment webHostEnvironment)
        {
            _teamService = teamService;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Returns a view with a list of all teams.
        /// </summary>
        /// <returns>View with team list</returns>
        public async Task<IActionResult> Index()
        {
            var teams = await _teamService.GetTeamsAsync();
            return View(teams);
        }

        /// <summary>
        /// Returns a view with detailed info for a team including its playlists.
        /// </summary>
        /// <param name="id">Team ID</param>
        /// <returns>TeamDetails view or NotFound</returns>
        public async Task<IActionResult> Details(int id)
        {
            var team = await _teamService.GetTeamDetailsAsync(id);
            if (team == null) return NotFound();

            var teams = await _teamService.GetTeamsAsync(); 
            ViewData["Teams"] = teams;

            return View(team);
        }

        /// <summary>
        /// Displays the form for creating a new team.
        /// </summary>
        /// <returns>Create view</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles POST request to create a new team.
        /// </summary>
        /// <param name="teamDto">Team data</param>
        /// <returns>Redirect to Index or re-render form with validation errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamDto teamDto, IFormFile logoFile)
        {
            if (!ModelState.IsValid) return View(teamDto);

            try
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                await _teamService.CreateTeamAsync(teamDto, logoFile, wwwRootPath);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(teamDto);
            }
        }


        /// <summary>
        /// Displays the form for editing an existing team.
        /// </summary>
        /// <param name="id">Team ID</param>
        /// <returns>Edit view or NotFound</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var team = await _teamService.GetTeamAsync(id);
            if (team == null) return NotFound();
            return View(team);
        }

        /// <summary>
        /// Handles POST request to update an existing team.
        /// </summary>
        /// <param name="id">Team ID</param>
        /// <param name="teamDto">Updated team data</param>
        /// <returns>Redirect to Index or re-render form</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamDto teamDto, IFormFile logoFile)
        {
            if (id != teamDto.id)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState); // important for AJAX

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            var updated = await _teamService.UpdateTeamAsync(id, teamDto, logoFile, wwwRootPath);

            if (updated == null)
                return NotFound();

            // Return JSON for AJAX success handling
            return Json(new
            {
                id = updated.id,
                name = updated.Name,
                logoUrl = updated.LogoUrl
            });
        }



        /// <summary>
        /// Displays the confirmation page for deleting a team.
        /// </summary>
        /// <param name="id">Team ID</param>
        /// <returns>Delete confirmation view or NotFound</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _teamService.GetTeamAsync(id);
            if (team == null) return NotFound();
            return View(team);
        }

        /// <summary>
        /// Handles AJAX POST request to delete a team.
        /// Returns a JSON result indicating success or failure.
        /// </summary>
        /// <param name="id">Team ID</param>
        /// <returns>JSON result with success status</returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _teamService.DeleteTeamAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
