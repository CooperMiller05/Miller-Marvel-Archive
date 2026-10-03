using MarvelArchive.Data;
using MarvelArchive.Models;
using MarvelArchive.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Character character)
        {
            var existingCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == character.Id);

            if (existingCharacter == null)
            {
                return NotFound();
            }

            existingCharacter.HeroName = character.HeroName;
            existingCharacter.RealName = character.RealName;
            existingCharacter.Aliases = character.Aliases;
            existingCharacter.PlaceOfOrigin = character.PlaceOfOrigin;
            existingCharacter.Gender = character.Gender;
            existingCharacter.Race = character.Race;
            existingCharacter.Alignment = character.Alignment;
            existingCharacter.Description = character.Description;
            existingCharacter.Intelligence = character.Intelligence;
            existingCharacter.Strength = character.Strength;
            existingCharacter.Speed = character.Speed;
            existingCharacter.Durability = character.Durability;
            existingCharacter.Power = character.Power;
            existingCharacter.Combat = character.Combat;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = character.Id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

            return View(character);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(Character character)
        {
            var existingCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == character.Id);

            if (existingCharacter == null)
            {
                return NotFound();
            }

            _context.Characters.Remove(existingCharacter);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create (Character character)
        {
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = character.Id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportCharacters()
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
        }
    }
}
