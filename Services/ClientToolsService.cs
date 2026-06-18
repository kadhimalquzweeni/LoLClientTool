using System.Diagnostics;

namespace LoLClientTool.Services
{
    public class ClientToolsService : IClientToolsService
    {
        public LeagueClientResult RestartLeagueUx()
        {
            string[] processNames =
            {
                "LeagueClientUx",
                "LeagueClientUxRender"
            };

            int killedProcessCount = 0;
            List<string> errors = new();

            foreach (string processName in processNames)
            {
                Process[] processes = Process.GetProcessesByName(processName);

                foreach (Process process in processes)
                {
                    try
                    {
                        process.Kill();
                        killedProcessCount++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"{processName}: {ex.Message}");
                    }
                    finally
                    {
                        process.Dispose();
                    }
                }
            }

            if (killedProcessCount == 0 && errors.Count == 0)
            {
                return new LeagueClientResult
                {
                    Success = false,
                    Message = "League UX processes were not found. Make sure the League Client is open."
                };
            }

            if (errors.Any())
            {
                return new LeagueClientResult
                {
                    Success = false,
                    Message = $"Restart attempted, but some processes could not be closed: {string.Join(" | ", errors)}"
                };
            }

            return new LeagueClientResult
            {
                Success = true,
                Message = $"Restarted League UX. Closed {killedProcessCount} process(es). The League interface should reopen shortly."
            };
        }
    }
}