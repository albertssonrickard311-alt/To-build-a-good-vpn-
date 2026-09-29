using System.Text.Json;
using VeytrixVPN.Models;

namespace VeytrixVPN.Services;

public class SettingsService
{
    private readonly string directory;
    private readonly string file;

    public SettingsService()
    {
        directory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "VeytrixVPN");

        file = Path.Combine(directory, "settings.json");

        Directory.CreateDirectory(directory);
    }

    public VpnSettings Load()
    {
        try
        {
            if (!File.Exists(file))
                return new VpnSettings();

            string json = File.ReadAllText(file);

            return JsonSerializer.Deserialize<VpnSettings>(json)
                   ?? new VpnSettings();
        }
        catch
        {
            return new VpnSettings();
        }
    }

    public void Save(VpnSettings settings)
    {
        Directory.CreateDirectory(directory);

        string json = JsonSerializer.Serialize(
            settings,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(file, json);
    }
}
