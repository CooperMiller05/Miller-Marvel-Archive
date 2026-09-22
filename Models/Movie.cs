using System.ComponentModel.DataAnnotations;

namespace MarvelArchive.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateOnly ReleaseDate { get; set; }
        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}
