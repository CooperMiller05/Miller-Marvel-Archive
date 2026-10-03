using System.ComponentModel.DataAnnotations;

namespace MarvelArchive.Models
{
    public class Media
    {
        public int Id { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        public DateOnly? ReleaseDate { get; set; }
        public long? BoxOffice {  get; set; }
        public int? Duration { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public string? TrailerUrl { get; set; } = string.Empty;
        public string? Director { get; set; } = string.Empty;
        public int? Phase {  get; set; }
        public string? Saga {  get; set; } = string.Empty;
        public int? Chronology { get; set; }
        public string Studio {  get; set; } = string.Empty;
        public string? MultiverseDesignation {  get; set; } = string.Empty;
        [Required]
        public bool IsMCU { get; set; }
        public int? Season { get; set; }
        public int? Episodes { get; set; }

        [Required]
        public bool IsVisible { get; set; } = true;
    }
}
