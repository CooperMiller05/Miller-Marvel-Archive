using MarvelArchive.Data;
using MarvelArchive.Models;
using MarvelArchive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MarvelArchive.ViewModels;
using System.Diagnostics;

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

        public async Task<IActionResult> Index(string? search, string alignment = "all", string sort = "atoz", int page = 1)
        {
            const int pageSize = 25;

            var allCharacters = await _context.Characters.AsNoTracking().Select(c => new CharacterViewModel
                {
                    Id = c.Id,
                    HeroName = c.HeroName,
                    RealName = c.RealName,
                    Aliases = c.Aliases,
                    Alignment = c.Alignment,
                    IsVisible = c.IsVisible
                }).ToListAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                allCharacters = allCharacters.Where(c =>
                        (c.HeroName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (c.RealName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (c.Aliases?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
            }

            if (alignment == "good")
            {
                allCharacters = allCharacters.Where(c => c.Alignment == "good").ToList();
            }
            else if (alignment == "bad")
            {
                allCharacters = allCharacters.Where(c => c.Alignment == "bad").ToList();
            }

            if (sort == "ztoa")
            {
                allCharacters = allCharacters.OrderByDescending(c => c.HeroName).ToList();
            }
            else
            {
                allCharacters = allCharacters.OrderBy(c => c.HeroName).ToList();
            }

            int totalCharacters = allCharacters.Count;

            int totalPages = (int)Math.Ceiling(totalCharacters / (double)pageSize);

            page = Math.Max(1, page);
            page = totalPages > 0 ? Math.Min(page, totalPages) : 1;

            var characters = allCharacters.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Search = search;
            ViewBag.Alignment = alignment;
            ViewBag.Sort = sort;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(characters);
        }

        public async Task<IActionResult> SearchSuggestions(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
            {
                return Json(new List<object>());
            }

            var suggestions = await _context.Characters
    .AsNoTracking()
    .Where(c => c.IsVisible &&
        (c.HeroName.Contains(term) ||
         c.RealName.Contains(term) ||
         c.Aliases.Contains(term)))
    .OrderBy(c =>
        c.HeroName.Contains(term) ? 0 :
        c.RealName.Contains(term) ? 1 : 2)
    .ThenBy(c => c.HeroName)
    .Take(5)
    .Select(c => new
    {
        c.Id,
        c.HeroName,
        c.RealName
    })
    .ToListAsync();

            return Json(suggestions);
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
