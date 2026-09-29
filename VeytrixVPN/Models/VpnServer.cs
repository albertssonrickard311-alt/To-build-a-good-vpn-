namespace VeytrixVPN.Models;

public class VpnServer
{
    public string Id { get; set; } = "";
    public string Country { get; set; } = "";
    public string City { get; set; } = "";
    public string Hostname { get; set; } = "";
    public string IPv4 { get; set; } = "";
    public string IPv6 { get; set; } = "";
    public int Port { get; set; } = 51820;
    public int Load { get; set; }
    public string PublicKey { get; set; } = "";

    public int Ping { get; set; } = -1;

    public string DisplayName =>
        $"{Country} — {City}";

    public string PingText =>
        Ping < 0 ? "—" : $"{Ping} ms";

    public string LoadText =>
        $"{Load}%";
}
