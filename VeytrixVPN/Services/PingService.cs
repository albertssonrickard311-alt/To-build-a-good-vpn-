using System.Net.NetworkInformation;
using VeytrixVPN.Models;

namespace VeytrixVPN.Services;

public class PingService
{
    public async Task<int> GetPingAsync(VpnServer server)
    {
        try
        {
            using Ping ping = new();

            PingReply result =
                await ping.SendPingAsync(
                    server.Hostname,
                    1500);

            if (result.Status != IPStatus.Success)
                return -1;

            return (int)result.RoundtripTime;
        }
        catch
        {
            return -1;
        }
    }
}
