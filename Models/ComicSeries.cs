namespace MarvelArchive.Models
{
    public class ComicSeries
    {
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public bool IsVisible { get; set; } = true;
        public ICollection<ComicIssue> ComicIssues { get; set; } = new List<ComicIssue>();
    }
}