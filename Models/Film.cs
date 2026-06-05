using System.Text.Json.Serialization;

namespace StarWarsBlazorApp.Models;

public class Film
{
    public string Title { get; set; } = "";

    [JsonPropertyName("episode_id")]
    public int EpisodeId { get; set; }

    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; } = "";

    public List<string> Starships { get; set; } = [];

    [JsonPropertyName("characters")]
    public List<string> Characters { get; set; } = [];

    public List<string> Planets { get; set; } = [];
}
