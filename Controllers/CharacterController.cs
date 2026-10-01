using MarvelArchive.Data;
using MarvelArchive.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MarvelArchive.Controllers
{
    public class CharacterController : Controller
    {
        private readonly SuperheroApiService _apiService;
        private readonly MarvelDbContext _context;

        public CharacterController(SuperheroApiService apiService, MarvelDbContext context)
        {
            _apiService = apiService;
            _context = context;
        }

        /* public async Task<IActionResult> TestCharacter()
        {
            var character = await _apiService.GetCharacterAsync(100);

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            return Ok(character);
        } */

        public async Task<IActionResult> Index()
        {
            var characters = await _context.Characters.ToListAsync();

            return View(characters);
        }

        public async Task<IActionResult> Details(int id)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        /* public async Task<IActionResult> ImportCharacters()
        {
            for (int id = 1; id <= 731; id++)
            {
                var character = await _apiService.GetCharacterAsync(id);

                if (character != null)
                {
                    var exists = await _context.Characters.AnyAsync(c => c.HeroName == character.HeroName);

                    if (exists == false)
                    {
                        _context.Characters.Add(character);
                    }
                }
            }
            await _context.SaveChangesAsync();

            return Ok("import complete");
        } */
    }
}
