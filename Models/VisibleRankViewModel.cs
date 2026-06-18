namespace LoLClientTool.Models
{
    public class VisibleRankViewModel
    {
        public string? ResultMessage { get; set; }

        public bool IsLeagueClientRunning { get; set; }

        public string Queue { get; set; } = "RANKED_SOLO_5x5";

        public string Tier { get; set; } = "GOLD";

        public string Division { get; set; } = "I";
    }
}