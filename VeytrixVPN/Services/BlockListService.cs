namespace VeytrixVPN.Services;

public class BlockListService
{
    public int CountRules(string fileName)
    {
        string file =
            Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                fileName);

        if (!File.Exists(file))
            return 0;

        return File.ReadLines(file)
            .Count(line =>
                !string.IsNullOrWhiteSpace(line) &&
                !line.TrimStart().StartsWith("#"));
    }

    public HashSet<string> LoadDomains(string fileName)
    {
        string file =
            Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                fileName);

        if (!File.Exists(file))
            return new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        return File.ReadLines(file)
            .Select(x => x.Trim())
            .Where(x =>
                !string.IsNullOrWhiteSpace(x) &&
                !x.StartsWith("#"))
            .ToHashSet(
                StringComparer.OrdinalIgnoreCase);
    }
}
