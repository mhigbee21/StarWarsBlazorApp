namespace StarWarsBlazorApp.Models;

public record FilmDashboardItem(
    string Title,
    int EpisodeId,
    int StarshipCount,
    int PeopleCount,
    int PlanetCount
);
