using MarvelArchive.Services;
using Microsoft.AspNetCore.Mvc;
using MarvelArchive.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> TestCharacter()
        {
            var character = await _apiService.GetCharacterAsync(100);

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            return Ok(character);
        }

        public async Task<IActionResult> Index()
        {
            var characters = await _context.Characters.ToListAsync();

            return View(characters);
        }
    }
}
