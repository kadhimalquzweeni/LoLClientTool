namespace LoLClientTool.Models
{
    public class ProfileBackgroundSkinListViewModel
    {
        public string ChampionId { get; set; } = string.Empty;

        public string ChampionName { get; set; } = string.Empty;

        public string? StatusMessage { get; set; }

        public bool IsLeagueClientRunning { get; set; }

        public List<ProfileBackgroundOptionViewModel> AvailableSkins { get; set; } = new();
    }
}