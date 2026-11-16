using MarvelArchive.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MarvelArchive.Services
{
    public class ComicApiService
    {
        private readonly HttpClient _httpClient;

        public ComicApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ComicSeries?> GetComicSeriesAsync(int id)
        {
            var response = await _httpClient.GetAsync(id.ToString());

            var json = await response.Content.ReadAsStringAsync();

            JsonDocument document = JsonDocument.Parse(json);

            ComicSeries comicSeries = new ComicSeries();

            comicSeries.Title = document.RootElement.GetProperty("series_name").ToString();

            return comicSeries;
        }

        public async Task<ComicIssue> GetComicIssuesAsync(int id)
        {
            var response = await _httpClient.GetAsync(id.ToString());

            var json = await response.Content.ReadAsStringAsync();

            JsonDocument document = JsonDocument.Parse(json);

            ComicIssue comicIssue = new ComicIssue();

            comicIssue.Title = document.RootElement.GetProperty("title").ToString();
            comicIssue.IssueNumber = int.Parse(document.RootElement.GetProperty("issueNumber").ToString());
            comicIssue.SeriesId = int.Parse(document.RootElement.GetProperty("seriesId").ToString());
            comicIssue.Description = document.RootElement.GetProperty("description").ToString();
        }
    }
}
