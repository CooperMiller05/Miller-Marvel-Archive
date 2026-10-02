using MarvelArchive.Models;
using System.Text.Json;
using System.Linq;

namespace MarvelArchive.Services
{
    public class SuperheroApiService
    {
        private readonly HttpClient _httpClient;

        public SuperheroApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Character?> GetCharacterAsync(int id)
        {
            var response = await _httpClient.GetAsync(id.ToString());

            var json = await response.Content.ReadAsStringAsync();

            JsonDocument document = JsonDocument.Parse(json);

            Character character = new Character();

            var publisher = document.RootElement.GetProperty("biography").GetProperty("publisher").GetString();

            var allowedPublishers = new List<string>
            {
                "Angel",
                "Angel Salvadore",
                "Anti-Venom",
                "Anti-Vision",
                "Ant-Man",
                "Archangel",
                "Arsenal",
                "Atlas",
                "Aztar",
                "Binary",
                "Blaquesmith",
                "Boom-Boom",
                "Captain Marvel",
                "Deadpool",
                "Evil Deadpool",
                "Gemini V",
                "Giant-Man",
                "Goliath",
                "Hawkeye",
                "Hawkfire",
                "Iron Lad",
                "Jean Grey",
                "Luke Cage",
                "Marvel Comics",
                "Meltdown",
                "Ms Marvel II",
                "Penance II",
                "Phoenix",
                "Power Man",
                "Rune King Thor",
                "Scarlet Spider",
                "Scorpion",
                "Sharon Carter",
                "She-Thing",
                "Speed Demon",
                "Speedball",
                "Spider-Carnage",
                "Thunderbird II",
                "Toxin",
                "Venom III",
                "Vindicator II",
                "Warpath"
            };

            if (allowedPublishers.Contains(publisher))
            {
                character.HeroName = document.RootElement.GetProperty("name").GetString();
                character.RealName = document.RootElement.GetProperty("biography").GetProperty("full-name").GetString();

                var aliases = document.RootElement.GetProperty("biography").GetProperty("aliases");
                var aliasList = aliases.EnumerateArray().Select(aliases => aliases.GetString());
                character.Aliases = string.Join(", ", aliasList);

                character.PlaceOfOrigin = document.RootElement.GetProperty("biography").GetProperty("place-of-birth").GetString();
                character.Gender = document.RootElement.GetProperty("appearance").GetProperty("gender").GetString();
                character.Race = document.RootElement.GetProperty("appearance").GetProperty("race").GetString();
                character.Alignment = document.RootElement.GetProperty("biography").GetProperty("alignment").GetString();
                character.Intelligence = ParsePowerstat(document.RootElement.GetProperty("powerstats").GetProperty("intelligence").GetString());
                character.Strength = ParsePowerstat(document.RootElement.GetProperty("powerstats").GetProperty("strength").GetString());
                character.Speed = ParsePowerstat(document.RootElement.GetProperty("powerstats").GetProperty("speed").GetString());
                character.Durability = ParsePowerstat(document.RootElement.GetProperty("powerstats").GetProperty("durability").GetString());
                character.Power = ParsePowerstat(document.RootElement.GetProperty("powerstats").GetProperty("power").GetString());
                character.Combat = ParsePowerstat(document.RootElement.GetProperty("powerstats").GetProperty("combat").GetString());
                character.Publisher = document.RootElement.GetProperty("biography").GetProperty("publisher").GetString();
                character.ImageURL = document.RootElement.GetProperty("image").GetProperty("url").GetString();

                return character;
            }

            return null;
        }

        public int ParsePowerstat(string stat)
        {
            if (int.TryParse(stat, out int statNum))
            {
                return statNum;
            }

            return 0;
        }
    }
}
