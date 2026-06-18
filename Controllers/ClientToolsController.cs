using LoLClientTool.Models;
using LoLClientTool.Mvc.Services;
using LoLClientTool.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoLProfileChanger.Mvc.Controllers
{
    public class ClientToolsController : Controller
    {
        private readonly ILeagueClientDetector _leagueClientDetector;
        private readonly IClientToolsService _clientToolsService;

        public ClientToolsController(
            ILeagueClientDetector leagueClientDetector,
            IClientToolsService clientToolsService)
        {
            _leagueClientDetector = leagueClientDetector;
            _clientToolsService = clientToolsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(BuildModel());
        }

        [HttpPost]
        public IActionResult RestartLeagueUx()
        {
            LeagueClientResult result =
                _clientToolsService.RestartLeagueUx();

            return View("Index", BuildModel(result.Message));
        }

        private ClientToolsViewModel BuildModel(string? resultMessage = null)
        {
            bool isLeagueClientRunning =
                _leagueClientDetector.IsLeagueClientRunning();

            return new ClientToolsViewModel
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