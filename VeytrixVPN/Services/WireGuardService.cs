using System.Diagnostics;

namespace VeytrixVPN.Services;

public class WireGuardService
{
    private readonly string wireGuardExe;

    public WireGuardService()
    {
        wireGuardExe = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "WireGuard",
            "wireguard.exe");
    }

    public bool IsInstalled()
    {
        return File.Exists(wireGuardExe);
    }

    public async Task<bool> InstallTunnelAsync(string configPath)
    {
        if (!File.Exists(configPath))
            return false;

        if (!IsInstalled())
            return false;

        string arguments =
            $"/installtunnelservice \"{configPath}\"";

        return await RunElevatedAsync(arguments);
    }

    public async Task<bool> RemoveTunnelAsync(string tunnelName)
    {
        if (!IsInstalled())
            return false;

        string arguments =
            $"/uninstalltunnelservice \"{tunnelName}\"";

        return await RunElevatedAsync(arguments);
    }

    private static async Task<bool> RunElevatedAsync(string arguments)
    {
        using Process process = new();

        process.StartInfo = new ProcessStartInfo
        {
            FileName = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ProgramFiles),
                "WireGuard",
                "wireguard.exe"),

            Arguments = arguments,

            UseShellExecute = true,

            Verb = "runas"
        };

        try
        {
            process.Start();

            await process.WaitForExitAsync();

            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}
