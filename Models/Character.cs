using Humanizer;
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
        public string Aliases { get; set; } = string.Empty;
        [Required]
        public string PlaceOfOrigin { get; set; } = string.Empty;
        [Required]
        public string Gender { get; set; } = string.Empty;
        [Required]
        public string Race { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Alignment { get; set; } = string.Empty;
        public int Intelligence { get; set; }
        public int Strength { get; set; }
        public int Speed { get; set; }
        public int Durability { get; set; }
        public int Power { get; set; }
        public int Combat { get; set; }
        public string? Publisher { get; set; }
        [Required]
        public string? ImageURL { get; set; }


        public string GetAlignmentValue()
        {
            if (Alignment == "good")
            {
                return "Hero";
            }
            else if (Alignment == "bad")
            {
                return "Villain";
            }
            else if (Alignment == "-")
            {
                return "Unknown";
            } 
            else
            {
                return Alignment.ApplyCase(LetterCasing.Title);
            }
        }
    }
}
