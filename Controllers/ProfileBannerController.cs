using LoLClientTool.Models;
using LoLClientTool.Mvc.Services;
using LoLClientTool.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoLProfileChanger.Mvc.Controllers
{
    public class ProfileBannerController : Controller
    {
        private readonly ILeagueClientDetector _leagueClientDetector;
        private readonly ILeagueClientService _leagueClientService;

        public ProfileBannerController(
            ILeagueClientDetector leagueClientDetector,
            ILeagueClientService leagueClientService)
        {
            _leagueClientDetector = leagueClientDetector;
            _leagueClientService = leagueClientService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(BuildModel());
        }

        [HttpPost]
        public async Task<IActionResult> SetLastRankBanner()
        {
            LeagueClientConnection? connection = _leagueClientDetector.GetConnection();

            bool isConnected = connection != null;

            if (!isConnected)
            {
                return View("Index", new ProfileBannerViewModel
                {
                    IsLeagueClientRunning = false,
                    ResultMessage = "League Client is not running, or the lockfile could not be read. Open League of Legends first, then try again."
                });
            }

            LeagueClientResult result =
                await _leagueClientService.SetLastRankBannerAsync();

            return View("Index", BuildModel(result.Message));
        }

        private ProfileBannerViewModel BuildModel(string? resultMessage = null)
        {
            LeagueClientConnection? connection = _leagueClientDetector.GetConnection();

            bool isConnected = connection != null;

            return new ProfileBannerViewModel
            {
                IsLeagueClientRunning = isConnected,
                ResultMessage = resultMessage
                    ?? (isConnected
                        ? $"League Client detected on port {connection!.Port}."
                        : "League Client is not running, or the lockfile could not be read. Open League of Legends first, then refresh this page.")
            };
        }
    }
}