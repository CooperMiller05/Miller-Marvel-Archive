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

        public async Task<Character> GetCharacterAsync(int id)
        {
            var response = await _httpClient.GetAsync(id.ToString());

            var json =  await response.Content.ReadAsStringAsync();

            JsonDocument document = JsonDocument.Parse(json);

            Character character = new Character();

            character.HeroName = document.RootElement.GetProperty("name").GetString();
            character.RealName = document.RootElement.GetProperty("biography").GetProperty("full-name").GetString();

            var aliases = document.RootElement.GetProperty("biography").GetProperty("aliases");
            var aliasList = aliases.EnumerateArray().Select(aliases => aliases.GetString());
            character.Aliases = string.Join(", ", aliasList);
            
            character.PlaceOfBirth = document.RootElement.GetProperty("biography").GetProperty("place-of-birth").GetString();
            character.Gender = document.RootElement.GetProperty("appearance").GetProperty("gender").GetString();
            character.Race = document.RootElement.GetProperty("appearance").GetProperty("race").GetString();
            character.Alignment = document.RootElement.GetProperty("biography").GetProperty("alignment").GetString();
            character.Intelligence = int.Parse(document.RootElement.GetProperty("powerstats").GetProperty("intelligence").GetString());
            character.Strength = int.Parse(document.RootElement.GetProperty("powerstats").GetProperty("strength").GetString());
            character.Speed = int.Parse(document.RootElement.GetProperty("powerstats").GetProperty("speed").GetString());
            character.Durability = int.Parse(document.RootElement.GetProperty("powerstats").GetProperty("durability").GetString());
            character.Power = int.Parse(document.RootElement.GetProperty("powerstats").GetProperty("power").GetString());
            character.Combat = int.Parse(document.RootElement.GetProperty("powerstats").GetProperty("combat").GetString());
            character.ImageURL = document.RootElement.GetProperty("image").GetProperty("url").GetString();

            return character;
        }
    }
}
