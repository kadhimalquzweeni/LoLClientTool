using LoLClientTool.Mvc.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LoLClientTool.Services
{
    public class LeagueClientService : ILeagueClientService
    {
        private readonly ILeagueClientDetector _leagueClientDetector;

        public LeagueClientService(ILeagueClientDetector leagueClientDetector)
        {
            _leagueClientDetector = leagueClientDetector;
        }

        public async Task<LeagueClientResult> SetProfileIconAsync(int iconId)
        {
            LeagueClientConnection? connection = _leagueClientDetector.GetConnection();

            if (connection == null)
            {
                return new LeagueClientResult
                {
                    Success = false,
                    Message = "League Client is not running, or the lockfile could not be read."
                };
            }

            try
            {
                using var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var httpClient = new HttpClient(handler);

                string credentials = $"riot:{connection.Password}";
                string encodedCredentials = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(credentials));

                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", encodedCredentials);

                string url =
                    $"{connection.Protocol}://127.0.0.1:{connection.Port}/lol-summoner/v1/current-summoner/icon";

                var payload = new
                {
                    profileIconId = iconId
                };

                string json = JsonSerializer.Serialize(payload);

                using var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage response = await httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return new LeagueClientResult
                    {
                        Success = true,
                        Message = $"Profile icon changed to {iconId}."
                    };
                }

                string responseBody = await response.Content.ReadAsStringAsync();

                return new LeagueClientResult
                {
                    Success = false,
                    Message = $"League Client rejected the request. Status: {(int)response.StatusCode}. Response: {responseBody}"
                };
            }
            catch (Exception ex)
            {
                return new LeagueClientResult
                {
                    Success = false,
                    Message = $"Failed to update profile icon: {ex.Message}"
                };
            }
        }

        public async Task<LeagueClientResult> SetProfileBackgroundAsync(int skinId)
        {
            LeagueClientConnection? connection = _leagueClientDetector.GetConnection();

            if (connection == null)
            {
                return new LeagueClientResult
                {
                    Success = false,
                    Message = "League Client is not running, or the lockfile could not be read."
                };
            }

            try
            {
                using var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var httpClient = new HttpClient(handler);

                string credentials = $"riot:{connection.Password}";
                string encodedCredentials = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(credentials));

                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", encodedCredentials);

                string url =
                    $"{connection.Protocol}://127.0.0.1:{connection.Port}/lol-summoner/v1/current-summoner/summoner-profile";

                var payload = new
                {
                    key = "backgroundSkinId",
                    value = skinId
                };

                string json = JsonSerializer.Serialize(payload);

                using var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return new LeagueClientResult
                    {
                        Success = true,
                        Message = $"Profile background changed to skin ID {skinId}."
                    };
                }

                string responseBody = await response.Content.ReadAsStringAsync();

                return new LeagueClientResult
                {
                    Success = false,
                    Message = $"League Client rejected the request. Status: {(int)response.StatusCode}. Response: {responseBody}"
                };
            }
            catch (Exception ex)
            {
                return new LeagueClientResult
                {
                    Success = false,
                    Message = $"Failed to update profile background: {ex.Message}"
                };
            }
        }
    }
}