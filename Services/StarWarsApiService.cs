using StarWarsBlazorApp.Models;
using System.Text.Json;

namespace StarWarsBlazorApp.Services;

public class StarWarsApiService(HttpClient http)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyList<Film>> GetAllFilmsAsync()
    {
        return await GetAsync<Film>("films");
    }
    public async Task<IReadOnlyList<StarShip>> GetAllStarshipsAsync()
    {
        return await GetAsync<StarShip>("starships");
    }

    public async Task<IReadOnlyList<FilmDashboardItem>> GetFilmDashboardAsync()
    {
        var films = await GetAllFilmsAsync();

        return films
            .OrderBy(f => f.EpisodeId)
            .Select(f => new FilmDashboardItem(
                f.Title,
                f.EpisodeId,
                f.Starships.Count,
                f.Characters.Count,
                f.Planets.Count
            ))
            .ToList();
    }

    private async Task<IReadOnlyList<T>> GetAsync<T>(string endpoint)
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