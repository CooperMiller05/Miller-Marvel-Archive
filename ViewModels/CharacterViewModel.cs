namespace MarvelArchive.ViewModels
{
    public class CharacterViewModel
    {
        public int Id { get; set; }
        public string HeroName { get; set; } = string.Empty;
        public string? RealName { get; set; }
        public string? Aliases { get; set; }
        public string? Alignment { get; set; }
        public bool IsVisible { get; set; }
    }
}