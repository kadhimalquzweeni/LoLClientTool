using LoLClientTool.Models;
using LoLClientTool.Mvc.Services;
using LoLClientTool.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoLProfileChanger.Mvc.Controllers
{
    public class FriendToolsController : Controller
    {
        private readonly ILeagueClientDetector _leagueClientDetector;
        private readonly IFriendToolsService _friendToolsService;

        public FriendToolsController(
            ILeagueClientDetector leagueClientDetector,
            IFriendToolsService friendToolsService)
        {
            _leagueClientDetector = leagueClientDetector;
            _friendToolsService = friendToolsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(BuildModel());
        }

        [HttpPost]
        public async Task<IActionResult> AcceptAllRequests()
        {
            LeagueClientResult result =
                await _friendToolsService.AcceptAllFriendRequestsAsync();

            return View("Index", BuildModel(result.Message));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllRequests()
        {
            LeagueClientResult result =
                await _friendToolsService.DeleteAllFriendRequestsAsync();

            return View("Index", BuildModel(result.Message));
        }

        private FriendToolsViewModel BuildModel(string? resultMessage = null)
        {
            bool isLeagueClientRunning =
                _leagueClientDetector.IsLeagueClientRunning();

            return new FriendToolsViewModel
            {
                IsLeagueClientRunning = isLeagueClientRunning,
                ResultMessage = resultMessage
                    ?? (isLeagueClientRunning
                        ? "League Client detected."
                        : "League Client is not running. Open League of Legends first, then refresh this page.")
            };
        }
    }
}