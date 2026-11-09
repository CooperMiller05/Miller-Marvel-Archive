using MarvelArchive.Data;
using MarvelArchive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MarvelArchive.Controllers
{
    public class CollectionController : Controller
    {
        private readonly MarvelDbContext _context;

        public CollectionController(MarvelDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var collections = await _context.Collections.Where(c => c.UserId == userId).ToListAsync();

            return View(collections);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var collection = await _context.Collections.Include(c => c.CollectionItems).ThenInclude(ci => ci.Character).Include(c => c.CollectionItems).ThenInclude(ci => ci.Media).FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == id);

            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Collection collection)
        {
            var existingCollection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == collection.Id);

            if (existingCollection == null)
            {
                return NotFound();
            }

            existingCollection.Name = collection.Name;
            existingCollection.Description = collection.Description;
            existingCollection.isPublic = collection.isPublic;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = collection.Id });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == id);

            return View(collection);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(Collection collection)
        {
            var existingCollection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == collection.Id);

            if (existingCollection == null)
            {
                return NotFound();
            }

            _context.Collections.Remove(existingCollection);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Collection collection)
        {
            _context.Collections.Add(collection);
            _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMyCollections()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var collections = await _context.Collections.Where(c => c.UserId == userId).Select(c => new
            {
                c.Id,
                c.Name
            }).ToListAsync();

            return Json(collections);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(int id, string type, int collectionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == collectionId && c.UserId == userId);

            if (collection == null)
            {
                return NotFound();
            }

            var item = new CollectionItem
            {
                CollectionId = collectionId
            };

            if (type == "character")
            {
                var characterExists = await _context.Characters.AnyAsync(c => c.Id == id);

                if (!characterExists)
                {
                    return NotFound();
                }

                var exists = await _context.CollectionItems.AnyAsync(ci => ci.CollectionId == collectionId && ci.CharacterId == id);

                if (exists == true)
                {
                    return BadRequest("Character is already in collection.");
                }

                item.CharacterId = id;
            }
            else if (type == "media")
            {
                var mediaExists = await _context.Media.AnyAsync(m => m.Id == id);

                if (!mediaExists)
                {
                    return NotFound();
                }

                var exists = await _context.CollectionItems.AnyAsync(ci => ci.CollectionId == collectionId && ci.MediaId == id);

                if (exists == true)
                {
                    return BadRequest("Media is already in collection.");
                }

                item.MediaId = id;
            }
            else
            {
                return BadRequest();
            }

            _context.CollectionItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}