namespace LoLClientTool.Services
{
    public interface ILeagueClientService
    {
        Task<LeagueClientResult> SetProfileIconAsync(int iconId);
        Task<LeagueClientResult> SetProfileBackgroundAsync(int skinId);
    }
}