using LoLClientTool.Models;

namespace LoLClientTool.Services
{
    public interface IProfileBackgroundAssetService
    {
        Task<List<ProfileBackgroundOptionViewModel>> GetBaseChampionBackgroundsAsync();

        Task<List<ProfileBackgroundOptionViewModel>> GetChampionSkinsAsync(string championId);
    }
}