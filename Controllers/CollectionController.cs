using MarvelArchive.Data;
using MarvelArchive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
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
        public async Task<IActionResult> AddItem(int Id, string type, int collectionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == collectionId && c.UserId == userId);

            CollectionItem item;

            if (collection == null)
            {
                return NotFound();
            }

            if (type == "character")
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == Id);

                if (character == null)
                {
                    return NotFound();
                }

                item = new CollectionItem
                {
                    CollectionId = collectionId,
                    CharacterId = Id
                };
                    
            }
            else if (type == "media")
            {
                var media = await _context.Media.FirstOrDefaultAsync(m => m.Id == Id);

                if (media == null)
                {
                    return NotFound();
                }

                item = new CollectionItem
                {
                    CollectionId = collectionId,
                    MediaId = Id
                };

            }
            else
            {
                return NotFound();
            }

            _context.CollectionItems.Add(item);
            await _context.SaveChangesAsync();

            string controller = type == "character" ? "Character" : "Media";

            return RedirectToAction("Index", controller);
        }
    }
}
