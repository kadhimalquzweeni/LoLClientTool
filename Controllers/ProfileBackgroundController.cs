using LoLClientTool.Models;
using LoLClientTool.Mvc.Services;
using LoLClientTool.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoLProfileChanger.Mvc.Controllers
{
    public class ProfileBackgroundController : Controller
    {
        private readonly ILeagueClientDetector _leagueClientDetector;
        private readonly ILeagueClientService _leagueClientService;
        private readonly IProfileBackgroundAssetService _profileBackgroundAssetService;

        public ProfileBackgroundController(
            ILeagueClientDetector leagueClientDetector,
            ILeagueClientService leagueClientService,
            IProfileBackgroundAssetService profileBackgroundAssetService)
        {
            _leagueClientDetector = leagueClientDetector;
            _leagueClientService = leagueClientService;
            _profileBackgroundAssetService = profileBackgroundAssetService;
        }

        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            LeagueClientConnection? connection = _leagueClientDetector.GetConnection();

            bool isConnected = connection != null;

            var model = new ProfileBackgroundViewModel
            {
                IsLeagueClientRunning = isConnected,
                AvailableBackgrounds = await _profileBackgroundAssetService.GetBaseChampionBackgroundsAsync(),
                StatusMessage = isConnected
                    ? $"League Client detected on port {connection!.Port}."
                    : "League Client is not running, or the lockfile could not be read. Open League of Legends first, then refresh this page."
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProfileBackgroundViewModel model)
        {
            LeagueClientConnection? connection = _leagueClientDetector.GetConnection();

            bool isLeagueClientRunning = connection != null;

            model.IsLeagueClientRunning = isLeagueClientRunning;
            model.AvailableBackgrounds = await _profileBackgroundAssetService.GetBaseChampionBackgroundsAsync();

            if (!ModelState.IsValid)
            {
                model.StatusMessage = "Please fix the validation errors.";
                return View(model);
            }


            if (!isLeagueClientRunning)
            {
                model.StatusMessage = "League Client is not running, or the lockfile could not be read. Open League of Legends first, then try again.";
                return View(model);
            }

            LeagueClientResult result =
                await _leagueClientService.SetProfileBackgroundAsync(model.SkinId!.Value);

            model.StatusMessage = result.Message;

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Skins(string championId)
        {
            LeagueClientConnection? connection = _leagueClientDetector.GetConnection();

            bool isConnected = connection != null;

            List<ProfileBackgroundOptionViewModel> skins =
                await _profileBackgroundAssetService.GetChampionSkinsAsync(championId);

            if (!skins.Any())
            {
                return NotFound();
            }

            var model = new ProfileBackgroundSkinListViewModel
            {
                ChampionId = championId,
                ChampionName = skins.First().ChampionName,
                AvailableSkins = skins,
                IsLeagueClientRunning = isConnected,
                StatusMessage = TempData["StatusMessage"] as string
                    ?? (isConnected
                        ? $"League Client detected on port {connection!.Port}."
                        : "League Client is not running, or the lockfile could not be read. Open League of Legends first, then refresh this page.")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SetSkin(int skinId, string championId)
        {
            LeagueClientConnection? connection = _leagueClientDetector.GetConnection();

            if (connection == null)
            {
                TempData["StatusMessage"] = "League Client is not running, or the lockfile could not be read. Open League of Legends first, then try again.";
                return RedirectToAction(nameof(Skins), new { championId });
            }

            LeagueClientResult result =
                await _leagueClientService.SetProfileBackgroundAsync(skinId);

            TempData["StatusMessage"] = result.Message;

            return RedirectToAction(nameof(Skins), new { championId });
        }
    }
}