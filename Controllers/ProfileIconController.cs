using LoLClientTool.Models; // Allows us to use ProfileIconViewModel
using LoLClientTool.Mvc.Services; // Allows us to use ILeagueClientDetector
using LoLClientTool.Services;
using Microsoft.AspNetCore.Mvc; // Gives us MVC classes like Controller and IActionResult

namespace LoLProfileChanger.Mvc.Controllers
{
    public class ProfileIconController : Controller // This is an MVC controller
    {
        private readonly ILeagueClientDetector _leagueClientDetector;
        private readonly ILeagueClientService _leagueClientService;

        public ProfileIconController(
            ILeagueClientDetector leagueClientDetector,
            ILeagueClientService leagueClientService)
        {
            _leagueClientDetector = leagueClientDetector;
            _leagueClientService = leagueClientService;
        }

        [HttpGet] // Runs when the user opens /profileicon
        public IActionResult Index() // The default action/page
        {
            LeagueClientConnection? connection = _leagueClientDetector.GetConnection(); // Check if the League Client is running

            bool isConnected = connection != null;
            var model = new ProfileIconViewModel // Create a new model for the page
            {
                IsLeagueClientRunning = isConnected,
                StatusMessage = isConnected ? $"League Client detected on port {connection!.Port}."
                : "League Client is not running, or the lockfile could not be read. Open League of Legends first, then refresh this page."
            };

            model.AvailableIcons = BuildIconList();
            return View(model); // Send the model to Views/ProfileIcon/Index.cshtml
        }

        [HttpPost] // Runs when the user submits the form
        public async Task<IActionResult> Index(ProfileIconViewModel model) // MVC fills this model from the form
        {
            LeagueClientConnection? connection = _leagueClientDetector.GetConnection();

            bool isLeagueClientRunning = connection != null;

            model.IsLeagueClientRunning = isLeagueClientRunning;
            model.AvailableIcons = BuildIconList();
            if (!ModelState.IsValid) // Check validation rules from the ViewModel
            {
                model.StatusMessage = "Please fix the validation errors."; // Set error status
                return View(model); // Return the page with validation errors
            }

            if (!isLeagueClientRunning) // Check if the League Client is running
            {
                model.StatusMessage = "League Client is not running, or the lockfile could not be read. Open League of Legends first, then try again.";
                return View(model); // Return the page with error message
            }

            LeagueClientResult result = await _leagueClientService.SetProfileIconAsync(model.IconId!.Value);

            model.StatusMessage = result.Message; // Set the status message from the result

            return View(model); // Return the page with the status message
        }

        private static List<ProfileIconOptionViewModel> BuildIconList()
        {
            const string dataDragonVersion = "15.24.1";

            return Enumerable.Range(50, 29)
                .Select(iconId => new ProfileIconOptionViewModel
                {
                    IconId = iconId,
                    ImageUrl = $"https://ddragon.leagueoflegends.com/cdn/{dataDragonVersion}/img/profileicon/{iconId}.png"
                })
                .ToList();
        }
    }
}