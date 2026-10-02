using System.ComponentModel.DataAnnotations;

namespace MarvelArchive.Models
{
    public class Media
    {
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int Phase {  get; set; }
        [Required]
        public string Saga {  get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateOnly ReleaseDate { get; set; }
        [Required]
        public DateOnly MCUTime {  get; set; }
        [Required]
        public string Genre { get; set; } = string.Empty;
        [Required]
        public string Runtime { get; set; } = string.Empty;
        [Required]
        public string ImageURL { get; set; } = string.Empty;
        [Required]
        public string Cast {  get; set; } = String.Empty;
        [Required]
        public string Director { get; set; } = string.Empty;
        [Required]
        public string Distribution { get; set; } = string.Empty;
        public string TrailerURL { get; set; } = string.Empty;
        [Required]
        public double IMDbRating {  get; set; }
        [Required]
        public string IMDbURL { get; set; } = string.Empty;
        public int? Seasons { get; set; }
        public int? Episodes { get; set; }
    }
}
