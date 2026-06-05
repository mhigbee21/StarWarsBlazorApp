using StarWarsBlazorApp.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace StarWarsBlazorApp.Services;

public class StarWarsApiService(HttpClient http)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyList<FilmDashboardItem>> GetFilmDashboardAsync()
    {
        var response = await http.GetFromJsonAsync<StarWarsListResponse<Film>>(
            "films/",
            JsonOptions
        );

        return response?.Results
            .OrderBy(f => f.EpisodeId)
            .Select(f => new FilmDashboardItem(
                f.Title,
                f.EpisodeId,
                f.Starships.Count,
                f.Characters.Count,
                f.Planets.Count
            ))
            .ToList() ?? [];
    }

    public async Task<IReadOnlyList<StarShip>> GetAllStarshipsAsync()
    {
        return await GetAllPagesAsync<StarShip>("starships/");
    }

    private async Task<IReadOnlyList<T>> GetAllPagesAsync<T>(string endpoint)
    {
        var results = new List<T>();
        string? nextUrl = endpoint;

        while (!string.IsNullOrWhiteSpace(nextUrl))
        {
            var page = await http.GetFromJsonAsync<StarWarsListResponse<T>>(
                nextUrl,
                JsonOptions
            );

            if (page is null)
            {
                break;
            }

            results.AddRange(page.Results);
            nextUrl = page.Next;
        }

        return results;
    }
}