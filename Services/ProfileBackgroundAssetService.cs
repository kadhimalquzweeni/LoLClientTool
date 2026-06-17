using LoLClientTool.Models;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace LoLClientTool.Services
{
    public class ProfileBackgroundAssetService : IProfileBackgroundAssetService
    {
        private const string DataDragonVersion = "15.24.1";
        private const string Language = "en_US";

        private readonly HttpClient _httpClient;

        public ProfileBackgroundAssetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProfileBackgroundOptionViewModel>> GetBaseChampionBackgroundsAsync()
        {
            string url =
                $"https://ddragon.leagueoflegends.com/cdn/{DataDragonVersion}/data/{Language}/champion.json";

            DataDragonChampionResponse? response =
                await _httpClient.GetFromJsonAsync<DataDragonChampionResponse>(url);

            if (response == null)
            {
                return new List<ProfileBackgroundOptionViewModel>();
            }

            return response.Data.Values
                .Select(champion =>
                {
                    if (!int.TryParse(champion.Key, out int championKey))
                    {
                        return null;
                    }

                    int baseSkinId = championKey * 1000;

                    return new ProfileBackgroundOptionViewModel
                    {
                        SkinId = baseSkinId,
                        ChampionId = champion.Id,
                        ChampionName = champion.Name,
                        SkinName = "Base",
                        ImageUrl = $"https://ddragon.leagueoflegends.com/cdn/img/champion/splash/{champion.Id}_0.jpg"
                    };
                })
                .Where(background => background != null)
                .Cast<ProfileBackgroundOptionViewModel>()
                .OrderBy(background => background.ChampionName)
                .ToList();
        }

        public async Task<List<ProfileBackgroundOptionViewModel>> GetChampionSkinsAsync(string championId)
        {
            string url =
                $"https://ddragon.leagueoflegends.com/cdn/{DataDragonVersion}/data/{Language}/champion/{championId}.json";

            DataDragonChampionDetailsResponse? response =
                await _httpClient.GetFromJsonAsync<DataDragonChampionDetailsResponse>(url);

            if (response == null || !response.Data.TryGetValue(championId, out DataDragonChampionDetails? champion))
            {
                return new List<ProfileBackgroundOptionViewModel>();
            }

            return champion.Skins
                .Select(skin =>
                {
                    if (!int.TryParse(skin.Id, out int skinId))
                    {
                        return null;
                    }

                    string skinName = skin.Name == "default"
                        ? "Base"
                        : skin.Name;

                    return new ProfileBackgroundOptionViewModel
                    {
                        SkinId = skinId,
                        ChampionId = champion.Id,
                        ChampionName = champion.Name,
                        SkinName = skinName,
                        ImageUrl = $"https://ddragon.leagueoflegends.com/cdn/img/champion/splash/{champion.Id}_{skin.Num}.jpg"
                    };
                })
                .Where(skin => skin != null)
                .Cast<ProfileBackgroundOptionViewModel>()
                .ToList();
        }

        private class DataDragonChampionResponse
        {
            [JsonPropertyName("data")]
            public Dictionary<string, DataDragonChampion> Data { get; set; } = new();
        }

        private class DataDragonChampion
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = string.Empty;

            [JsonPropertyName("key")]
            public string Key { get; set; } = string.Empty;

            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
        }

        private class DataDragonChampionDetailsResponse
        {
            [JsonPropertyName("data")]
            public Dictionary<string, DataDragonChampionDetails> Data { get; set; } = new();
        }

        private class DataDragonChampionDetails
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = string.Empty;

            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;

            [JsonPropertyName("skins")]
            public List<DataDragonSkin> Skins { get; set; } = new();
        }

        private class DataDragonSkin
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = string.Empty;

            [JsonPropertyName("num")]
            public int Num { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
        }
    }
}