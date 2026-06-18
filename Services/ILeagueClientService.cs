namespace LoLClientTool.Services
{
    public interface ILeagueClientService
    {
        Task<LeagueClientResult> SetProfileIconAsync(int iconId);
        Task<LeagueClientResult> SetProfileBackgroundAsync(int skinId);
        Task<LeagueClientResult> UpdateStatusMessageAsync(string statusMessage);
        Task<LeagueClientResult> ClearChallengeTokensAsync();
        Task<LeagueClientResult> CopyFirstChallengeTokenToAllSlotsAsync();
        Task<LeagueClientResult> SetLastRankBannerAsync();
        Task<LeagueClientResult> SetVisibleRankAsync(
    string queue,
    string tier,
    string division);
        Task<LeagueClientResult> ClearVisibleRankAsync();
    }
}