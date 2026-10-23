using MarvelArchive.Data;
using MarvelArchive.Models;
using MarvelArchive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarvelArchive.Controllers
{
    public class AdminController : Controller
    {

        private readonly MarvelDbContext _context;

        public AdminController(MarvelDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddConnection(int? characterId, int? mediaId)
        {
            ViewBag.Characters = await _context.Characters.ToListAsync();
            ViewBag.Media = await _context.Media.ToListAsync();

            ViewBag.SelectedCharacterId = characterId;
            ViewBag.SelectedMediaId = mediaId;

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddConnection(int characterId, int mediaId)
        {
            CharacterMedia characterMedia = new CharacterMedia(characterId, mediaId);

            _context.CharacterMedia.Add(characterMedia);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }
    }
}
