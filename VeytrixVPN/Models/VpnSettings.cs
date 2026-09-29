namespace VeytrixVPN.Models;

public class VpnSettings
{
    public bool AdBlock { get; set; } = true;

    public bool TrackerBlock { get; set; } = true;

    public bool AutoReconnect { get; set; } = true;

    public bool KillSwitch { get; set; } = true;

    public bool IPv6 { get; set; } = true;

    public string LastServerId { get; set; } = "";
}
