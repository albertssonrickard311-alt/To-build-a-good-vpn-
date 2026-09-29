using System.Text.Json;
using VeytrixVPN.Models;

namespace VeytrixVPN.Services;

public class ServerService
{
    public async Task<List<VpnServer>> LoadServersAsync()
    {
        string file = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "servers.json");

        if (!File.Exists(file))
            return new List<VpnServer>();

        string json = await File.ReadAllTextAsync(file);

        return JsonSerializer.Deserialize<List<VpnServer>>(json)
               ?? new List<VpnServer>();
    }
}
