using LoLClientTool.Models;
using LoLClientTool.Mvc.Services;
using LoLClientTool.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoLProfileChanger.Mvc.Controllers
{
    public class VisibleRankController : Controller
    {
        private readonly ILeagueClientDetector _leagueClientDetector;
        private readonly ILeagueClientService _leagueClientService;

        public VisibleRankController(
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
        public async Task<IActionResult> SetRank(VisibleRankViewModel model)
        {
            LeagueClientConnection? connection = _leagueClientDetector.GetConnection();

            if (connection == null)
            {
                model.IsLeagueClientRunning = false;
                model.ResultMessage = "League Client is not running, or the lockfile could not be read.";
                return View("Index", model);
            }

            LeagueClientResult result =
                await _leagueClientService.SetVisibleRankAsync(
                    model.Queue,
                    model.Tier,
                    model.Division);

            VisibleRankViewModel updatedModel = BuildModel(result.Message);
            updatedModel.Queue = model.Queue;
            updatedModel.Tier = model.Tier;
            updatedModel.Division = model.Division;

            return View("Index", updatedModel);
        }

        [HttpPost]
        public async Task<IActionResult> ClearRank()
        {
            LeagueClientResult result =
                await _leagueClientService.ClearVisibleRankAsync();

            return View("Index", BuildModel(result.Message));
        }

        private VisibleRankViewModel BuildModel(string? resultMessage = null)
        {
            LeagueClientConnection? connection = _leagueClientDetector.GetConnection();

            bool isConnected = connection != null;

            return new VisibleRankViewModel
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