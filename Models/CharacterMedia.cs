namespace MarvelArchive.Models
{
    public class CharacterMedia
    {
        public CharacterMedia (int characterId, int mediaId)
        {
            CharacterId = characterId;
            MediaId = mediaId;
        }

        public int CharacterId { get; set; }
        public Character Character { get; set; } = null!;
        public int MediaId { get; set; }
        public Media Media { get; set; } = null!;
    }
}
