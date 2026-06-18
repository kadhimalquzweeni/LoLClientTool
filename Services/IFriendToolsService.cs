namespace LoLClientTool.Services
{
    public interface IFriendToolsService
    {
        Task<LeagueClientResult> AcceptAllFriendRequestsAsync();

        Task<LeagueClientResult> DeleteAllFriendRequestsAsync();
    }
}