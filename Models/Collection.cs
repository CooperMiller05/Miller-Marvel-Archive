using System.ComponentModel.DataAnnotations;

namespace MarvelArchive.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool isPublic { get; set; } = false;
    }
}
