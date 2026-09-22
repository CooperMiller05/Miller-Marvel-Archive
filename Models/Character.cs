using System.ComponentModel.DataAnnotations;

namespace MarvelArchive.Models
{
    public class Character
    {
        public int Id { get; set; }
        [Required]
        public string HeroName { get; set; } = string.Empty;
        [Required]
        public string RealName { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Alignment { get; set; } = string.Empty;
        [Required]
        public string[] Powers { get; set; } = Array.Empty<string>();
        public string? ImageURL { get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
