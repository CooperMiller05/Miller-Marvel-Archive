namespace MarvelArchive.Models
{
    public class CharacterMedia
    {
        public int CharacterId { get; set; }
        public Character Character { get; set; } = null!;
        public int MediaId { get; set; }
        public Media Media { get; set; } = null!;
    }
}
