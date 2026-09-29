namespace VeytrixVPN.Services;

public static class LicenseService
{
    public const string Company =
        "Veytrix Games";

    public const string Product =
        "Veytrix VPN";

    public const string Copyright =
        "Copyright © 2026 Veytrix Games. All rights reserved.";

    public const string Ownership =
        "Veytrix VPN is developed and distributed by Veytrix Games.";

    public const string Warning =
        "Unauthorized redistribution, modification, or false claims of authorship are not permitted.";

    public static string FullNotice =>
        $"{Product}\n\n" +
        $"{Ownership}\n\n" +
        $"{Copyright}\n\n" +
        $"{Warning}";
}
