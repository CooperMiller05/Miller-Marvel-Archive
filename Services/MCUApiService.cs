using MarvelArchive.Models;
using System.Text.Json;

namespace MarvelArchive.Services
{
    public class MCUApiService
    {
        private readonly HttpClient _httpClient;

        public MCUApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Media?> GetMediaAsync(int id, string type)
        {
            var response = await _httpClient.GetAsync($"{type}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            JsonDocument document = JsonDocument.Parse(json);

            Media media = new Media();

            media.Type = document.RootElement.GetProperty("type").GetString();
            media.Title = document.RootElement.GetProperty("title").GetString();

            var releaseDate = document.RootElement.GetProperty("release_date");

            if (releaseDate.ValueKind != JsonValueKind.Null)
            {
                media.ReleaseDate = DateOnly.Parse(releaseDate.GetString());
            }

            if (type == "movies")
            {
                var boxOffice = document.RootElement.GetProperty("box_office");

                if (boxOffice.ValueKind != JsonValueKind.Null)
                {
                    media.BoxOffice = long.Parse(boxOffice.GetString());
                }
            
                var duration = document.RootElement.GetProperty("duration");

                if (duration.ValueKind != JsonValueKind.Null)
                {
                    media.Duration = duration.GetInt32();
                }
            }
            
            media.Description = document.RootElement.GetProperty("overview").GetString();
            media.ImageUrl = document.RootElement.GetProperty("cover_url").GetString();
            media.TrailerUrl = document.RootElement.GetProperty("trailer_url").GetString();
            media.Director = document.RootElement.GetProperty("directed_by").GetString();

            var phase = document.RootElement.GetProperty("phase");

            if (phase.ValueKind != JsonValueKind.Null)
            {
                media.Phase = phase.GetInt32();
            }

            media.Saga = document.RootElement.GetProperty("saga").GetString();

            var chronology = document.RootElement.GetProperty("chronology");

            if (chronology.ValueKind != JsonValueKind.Null)
            {
                media.Chronology = chronology.GetInt32();
            }

            media.Studio = document.RootElement.GetProperty("studio").GetString();
            media.MultiverseDesignation = document.RootElement.GetProperty("multiverse_designation").GetString();
            media.IsMCU = document.RootElement.GetProperty("is_mcu").GetBoolean();

            if (type == "tvshows")
            {
                var season = document.RootElement.GetProperty("season");

                if (season.ValueKind != JsonValueKind.Null)
                {
                    media.Season = season.GetInt32();
                }

                var episodes = document.RootElement.GetProperty("number_episodes");

                if (episodes.ValueKind != JsonValueKind.Null)
                {
                    media.Episodes = episodes.GetInt32();
                }
            }

            return media;
        }
    }
}
