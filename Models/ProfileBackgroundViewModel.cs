using System.ComponentModel.DataAnnotations;

namespace LoLClientTool.Models
{
    public class ProfileBackgroundViewModel
    {
        [Required(ErrorMessage = "Please enter a background skin ID.")]
        [Range(1, int.MaxValue, ErrorMessage = "Background skin ID must be a positive number.")]
        public int? SkinId { get; set; }

        public string? StatusMessage { get; set; }

        public bool IsLeagueClientRunning { get; set; }
        public List<ProfileBackgroundOptionViewModel> AvailableBackgrounds { get; set; } = new();
    }
}