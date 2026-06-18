using LoLClientTool.Mvc.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        public async Task<LeagueClientResult> UpdateStatusMessageAsync(string statusMessage)
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
                    $"{connection.Protocol}://127.0.0.1:{connection.Port}/lol-chat/v1/me";

                var payload = new
                {
                    statusMessage = statusMessage
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
                        Message = "Profile status message updated successfully."
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
                    Message = $"Failed to update status message: {ex.Message}"
                };
            }
        }
        public async Task<LeagueClientResult> ClearChallengeTokensAsync()
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
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedCredentials);

                string url =
                    $"{connection.Protocol}://127.0.0.1:{connection.Port}/lol-challenges/v1/update-player-preferences";

                var payload = new
                {
                    challengeIds = Array.Empty<int>()
                };

                string json = System.Text.Json.JsonSerializer.Serialize(payload);

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
                        Message = "Challenge tokens cleared successfully. You may need to refresh or restart the League Client to see the change."
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
                    Message = $"Failed to clear challenge tokens: {ex.Message}"
                };
            }
        }
        public async Task<LeagueClientResult> CopyFirstChallengeTokenToAllSlotsAsync()
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

                string baseUrl = $"{connection.Protocol}://127.0.0.1:{connection.Port}";

                string summaryUrl =
                    $"{baseUrl}/lol-challenges/v1/summary-player-data/local-player";

                HttpResponseMessage summaryResponse = await httpClient.GetAsync(summaryUrl);

                if (!summaryResponse.IsSuccessStatusCode)
                {
                    string responseBody = await summaryResponse.Content.ReadAsStringAsync();

                    return new LeagueClientResult
                    {
                        Success = false,
                        Message = $"Could not read challenge data. Status: {(int)summaryResponse.StatusCode}. Response: {responseBody}"
                    };
                }

                string summaryJson = await summaryResponse.Content.ReadAsStringAsync();

                using JsonDocument document = JsonDocument.Parse(summaryJson);

                JsonElement root = document.RootElement;

                if (!root.TryGetProperty("topChallenges", out JsonElement topChallenges)
                    || topChallenges.ValueKind != JsonValueKind.Array
                    || topChallenges.GetArrayLength() == 0)
                {
                    return new LeagueClientResult
                    {
                        Success = false,
                        Message = "No equipped challenge token was found. Equip a token in the League Client first, save it, close the identity customisation window, then try again."
                    };
                }

                JsonElement firstChallenge = topChallenges[0];

                if (!firstChallenge.TryGetProperty("id", out JsonElement firstChallengeIdElement))
                {
                    return new LeagueClientResult
                    {
                        Success = false,
                        Message = "The first equipped challenge token did not contain an ID."
                    };
                }

                int firstTokenId;

                if (firstChallengeIdElement.ValueKind == JsonValueKind.Number)
                {
                    firstTokenId = firstChallengeIdElement.GetInt32();
                }
                else if (firstChallengeIdElement.ValueKind == JsonValueKind.String
                         && int.TryParse(firstChallengeIdElement.GetString(), out int parsedId))
                {
                    firstTokenId = parsedId;
                }
                else
                {
                    return new LeagueClientResult
                    {
                        Success = false,
                        Message = "Could not parse the first challenge token ID."
                    };
                }

                string? titleId = null;

                if (root.TryGetProperty("title", out JsonElement titleElement)
                    && titleElement.ValueKind == JsonValueKind.Object
                    && titleElement.TryGetProperty("itemId", out JsonElement titleIdElement))
                {
                    titleId = titleIdElement.ToString();
                }

                string? bannerId = null;

                if (root.TryGetProperty("bannerId", out JsonElement bannerElement))
                {
                    bannerId = bannerElement.ToString();
                }

                var payload = new Dictionary<string, object?>
                {
                    ["challengeIds"] = new[]
                    {
                firstTokenId,
                firstTokenId,
                firstTokenId
            }
                };

                if (!string.IsNullOrWhiteSpace(titleId) && titleId != "-1")
                {
                    payload["title"] = titleId;
                }

                if (!string.IsNullOrWhiteSpace(bannerId))
                {
                    payload["bannerAccent"] = bannerId;
                }

                string updateJson = JsonSerializer.Serialize(payload);

                using var content = new StringContent(
                    updateJson,
                    Encoding.UTF8,
                    "application/json");

                string updateUrl =
                    $"{baseUrl}/lol-challenges/v1/update-player-preferences/";

                HttpResponseMessage updateResponse =
                    await httpClient.PostAsync(updateUrl, content);

                if (updateResponse.IsSuccessStatusCode)
                {
                    return new LeagueClientResult
                    {
                        Success = true,
                        Message = $"Copied challenge token {firstTokenId} into all 3 slots."
                    };
                }

                string updateResponseBody = await updateResponse.Content.ReadAsStringAsync();

                return new LeagueClientResult
                {
                    Success = false,
                    Message = $"League Client rejected the update. Status: {(int)updateResponse.StatusCode}. Response: {updateResponseBody}"
                };
            }
            catch (Exception ex)
            {
                return new LeagueClientResult
                {
                    Success = false,
                    Message = $"Failed to copy challenge token: {ex.Message}"
                };
            }
        }

        public async Task<LeagueClientResult> SetLastRankBannerAsync()
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

                string baseUrl = $"{connection.Protocol}://127.0.0.1:{connection.Port}";

                string summaryUrl =
                    $"{baseUrl}/lol-challenges/v1/summary-player-data/local-player";

                HttpResponseMessage summaryResponse = await httpClient.GetAsync(summaryUrl);

                if (!summaryResponse.IsSuccessStatusCode)
                {
                    string responseBody = await summaryResponse.Content.ReadAsStringAsync();

                    return new LeagueClientResult
                    {
                        Success = false,
                        Message = $"Could not read current profile preferences. Status: {(int)summaryResponse.StatusCode}. Response: {responseBody}"
                    };
                }

                string summaryJson = await summaryResponse.Content.ReadAsStringAsync();

                using JsonDocument document = JsonDocument.Parse(summaryJson);

                JsonElement root = document.RootElement;

                List<int> challengeIds = new();

                if (root.TryGetProperty("topChallenges", out JsonElement topChallenges)
                    && topChallenges.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement challenge in topChallenges.EnumerateArray())
                    {
                        if (challenge.TryGetProperty("id", out JsonElement idElement))
                        {
                            if (idElement.ValueKind == JsonValueKind.Number)
                            {
                                challengeIds.Add(idElement.GetInt32());
                            }
                            else if (idElement.ValueKind == JsonValueKind.String
                                     && int.TryParse(idElement.GetString(), out int parsedId))
                            {
                                challengeIds.Add(parsedId);
                            }
                        }
                    }
                }

                string? titleId = null;

                if (root.TryGetProperty("title", out JsonElement titleElement)
                    && titleElement.ValueKind == JsonValueKind.Object
                    && titleElement.TryGetProperty("itemId", out JsonElement titleIdElement))
                {
                    titleId = titleIdElement.ToString();
                }

                var payload = new Dictionary<string, object?>
                {
                    ["bannerAccent"] = "2"
                };

                if (challengeIds.Any())
                {
                    payload["challengeIds"] = challengeIds;
                }

                if (!string.IsNullOrWhiteSpace(titleId) && titleId != "-1")
                {
                    payload["title"] = titleId;
                }

                string updateJson = JsonSerializer.Serialize(payload);

                using var content = new StringContent(
                    updateJson,
                    Encoding.UTF8,
                    "application/json");

                string updateUrl =
                    $"{baseUrl}/lol-challenges/v1/update-player-preferences/";

                HttpResponseMessage updateResponse =
                    await httpClient.PostAsync(updateUrl, content);

                if (updateResponse.IsSuccessStatusCode)
                {
                    return new LeagueClientResult
                    {
                        Success = true,
                        Message = "Profile banner changed to the last-rank banner. You may need to refresh your profile to see it."
                    };
                }

                string updateResponseBody = await updateResponse.Content.ReadAsStringAsync();

                return new LeagueClientResult
                {
                    Success = false,
                    Message = $"League Client rejected the banner update. Status: {(int)updateResponse.StatusCode}. Response: {updateResponseBody}"
                };
            }
            catch (Exception ex)
            {
                return new LeagueClientResult
                {
                    Success = false,
                    Message = $"Failed to update profile banner: {ex.Message}"
                };
            }
        }
  
    public async Task<LeagueClientResult> SetVisibleRankAsync(
    string queue,
    string tier,
    string division)
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
                    $"{connection.Protocol}://127.0.0.1:{connection.Port}/lol-chat/v1/me";

                var payload = new
                {
                    lol = new
                    {
                        rankedLeagueQueue = queue,
                        rankedLeagueTier = tier,
                        rankedLeagueDivision = division
                    }
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
                        Message = $"Visible rank changed to {tier} {division} for {queue}."
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
                    Message = $"Failed to update visible rank: {ex.Message}"
                };
            }
        }
        public async Task<LeagueClientResult> ClearVisibleRankAsync()
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
                    $"{connection.Protocol}://127.0.0.1:{connection.Port}/lol-chat/v1/me";

                var payload = new
                {
                    lol = new
                    {
                        rankedLeagueQueue = "",
                        rankedLeagueTier = "",
                        rankedLeagueDivision = ""
                    }
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
                        Message = "Visible rank cleared."
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
                    Message = $"Failed to clear visible rank: {ex.Message}"
                };
            }
        }
    }

}