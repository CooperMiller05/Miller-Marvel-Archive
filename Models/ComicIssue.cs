using System.ComponentModel.DataAnnotations;

namespace MarvelArchive.Models
{
    public class ComicIssue
    {
        public int Id { get; set; }
        public int? SeriesId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? IssueNumber { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateOnly ReleaseDate { get; set; }
        public string? Creators { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        [Required]
        public bool IsVisible { get; set; } = true;
    }
}
