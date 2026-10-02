using MarvelArchive.Models;
using System.Text.Json;

namespace MarvelArchive.Services
{
    public class MCUTimelineApiService
    {
        private readonly HttpClient _httpClient;

        public MCUTimelineApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Media> GetMediaAsync(int id)
        {
            var response = await _httpClient.GetAsync(id.ToString());

            var json = await response.Content.ReadAsStringAsync();

            JsonDocument document = JsonDocument.Parse(json);

            Media media = new Media();

            media.CategoryId = int.Parse(document.RootElement.GetProperty("cid").GetString());
            media.Phase = int.Parse(document.RootElement.GetProperty("phase").GetString());
            media.Saga = document.RootElement.GetProperty("collectionName").GetString();
            media.Title = document.RootElement.GetProperty("title").GetString();
            media.Description = document.RootElement.GetProperty("description").GetString();
            media.ReleaseDate = DateOnly.Parse(document.RootElement.GetProperty("premiere").GetString());
            media.MCUTime = DateOnly.Parse(document.RootElement.GetProperty("mcutime").GetString());

            var genres = document.RootElement.GetProperty("imdbcache").GetProperty("genres");
            var genresList = genres.EnumerateArray().Select(genres => genres.GetString());
            media.Genre = string.Join(", ", genresList);

            media.Runtime = document.RootElement.GetProperty("runtime").GetString();
            media.ImageURL = document.RootElement.GetProperty("imdbcache").GetProperty("image").GetString();
            media.Distribution = document.RootElement.GetProperty("distribution").GetString();
            media.
        }
    }
}
