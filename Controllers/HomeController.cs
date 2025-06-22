using System.Diagnostics;
using System.Threading.Tasks;
using LockerRoomVibesCms.Interfaces;
using LockerRoomVibesCms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LockerRoomVibesCms.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITeamService _teamService;
        private readonly IPlaylistService _playlistService;

        public HomeController(
            ILogger<HomeController> logger,
            ITeamService teamService,
            IPlaylistService playlistService)
        {
            _logger = logger;
            _teamService = teamService;
            _playlistService = playlistService;
        }

        public async Task<IActionResult> Index()
        {
            var latestTeams = await _teamService.GetLatestTeamsAsync(2);
            var latestPlaylists = await _playlistService.GetLatestPlaylistsAsync(2);

            var viewModel = new HomeViewModel
            {
                LatestTeams = latestTeams,
                LatestPlaylists = latestPlaylists
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
