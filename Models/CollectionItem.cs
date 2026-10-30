namespace MarvelArchive.Models
{
    public class CollectionItem
    {
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public int? CharacterId { get; set; }
        public int? MediaId { get; set; }
        public Collection Collection { get; set; } = null!;
        public Character? Character { get; set; }
        public Media? Media { get; set; }
    }
}
