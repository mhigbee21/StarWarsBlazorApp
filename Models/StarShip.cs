using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace StarWarsBlazorApp.Models;

public class StarShip
{
    public string Name { get; set; } = "";
    public string Model { get; set; } = "";
    public string Manufacturer { get; set; } = "";
    public string Crew { get; set; } = "";
    [JsonPropertyName("cargo_capacity")]
    public string CargoCapacity { get; set; } = "";
    [JsonPropertyName("starship_class")]
    public string StarshipClass { get; set; } = "";
    public long? CrewNumber => ParseSwapiNumber(Crew);
    public long? CargoCapacityNumber => ParseSwapiNumber(CargoCapacity);

    private static long? ParseSwapiNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value
            .Replace(",", "")
            .Trim()
            .ToLowerInvariant();

        if (normalized is "unknown" or "n/a" or "none" or "indefinite")
        {
            return null;
        }

        var matches = Regex.Matches(normalized, @"\d+");

        if (matches.Count == 0)
        {
            return null;
        }

        return matches
            .Select(m => long.TryParse(m.Value, out var n) ? n : 0)
            .Max();
    }
}
