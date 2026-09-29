using System.Security.Cryptography;

namespace VeytrixVPN.Services;

public class IntegrityService
{
    public static string CalculateSha256(string file)
    {
        using FileStream stream = File.OpenRead(file);

        byte[] hash = SHA256.HashData(stream);

        return Convert.ToHexString(hash);
    }

    public static bool VerifySha256(
        string file,
        string expectedHash)
    {
        if (!File.Exists(file))
            return false;

        string actualHash = CalculateSha256(file);

        return string.Equals(
            actualHash,
            expectedHash,
            StringComparison.OrdinalIgnoreCase);
    }
}
