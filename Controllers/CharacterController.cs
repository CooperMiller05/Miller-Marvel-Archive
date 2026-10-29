using MarvelArchive.Data;
using MarvelArchive.Models;
using MarvelArchive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MarvelArchive.Controllers
{
    public class CharacterController : Controller
    {
        private readonly SuperheroApiService _superheroApiService;
        private readonly MCUApiService _mcuApiService;
        private readonly MarvelDbContext _context;

        public CharacterController(SuperheroApiService superheroApiService, MCUApiService mcuApiService, MarvelDbContext context)
        {
            _superheroApiService = superheroApiService;
            _mcuApiService = mcuApiService;
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, string alignment = "all", string sort = "atoz", int page  = 1)
        {
            page = Math.Max(1, page);
            const int pageSize = 25;

            var query = _context.Characters.AsNoTracking();

            if (!User.IsInRole("Admin"))
            {
                query = query.Where(c => c.IsVisible);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(c => c.HeroName.Contains(search) || (c.RealName != null && c.RealName.Contains(search)) || (c.Aliases != null && c.Aliases.Contains(search)));
            }

            if (alignment == "good")
            {
                query = query.Where(c => c.Alignment == "good");
            } else if (alignment == "bad")
            {
                query = query.Where(c => c.Alignment == "bad");
            }

            if (sort == "ztoa")
            {
                query = query.OrderByDescending(c => c.HeroName);
            } else
            {
                query = query.OrderBy(c => c.HeroName);
            }

            int totalCharacters = await query.CountAsync();

            var characters = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.Search = search;
            ViewBag.Alignment = alignment;
            ViewBag.Sort = sort;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCharacters / (double)pageSize);

            return View(characters);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters.Include(c => c.CharacterMedia).ThenInclude(cm => cm.Media).FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
            {
                return NotFound();
            }

            if (!character.IsVisible && !User.IsInRole("Admin"))
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
            existingCharacter.IsVisible = character.IsVisible;

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
        public async Task<IActionResult> ImportSuperheroCharacters()
        {
            for (int id = 1; id <= 731; id++)
            {
                var character = await _superheroApiService.GetCharacterAsync(id);

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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportCharacters()
        {
            for (int id = 1; id <= 731; id++)
            {
                var character = await _superheroApiService.GetCharacterAsync(id);

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
