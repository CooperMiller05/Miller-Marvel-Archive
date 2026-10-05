using MarvelArchive.Data;
using MarvelArchive.Models;
using MarvelArchive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarvelArchive.Controllers
{
    public class MediaController : Controller
    {
        private readonly MCUApiService _apiService;
        private readonly MarvelDbContext _context;

        public MediaController(MCUApiService apiService, MarvelDbContext context)
        {
            _apiService = apiService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var media = await _context.Media.ToListAsync();

            if (!User.IsInRole("Admin"))
            {
                media = media.Where(m => m.IsVisible).ToList();
            }

            return View(media);
        }

        public async Task<IActionResult> Details(int id)
        {
            var media = await _context.Media.FirstOrDefaultAsync(m => m.Id == id);

            if (media == null)
            {
                return NotFound();
            }

            if (!media.IsVisible && !User.IsInRole("Admin"))
            {
                return NotFound();
            }

            return View(media);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var media = await _context.Media.FirstOrDefaultAsync(m => m.Id == id);

            if (media == null)
            {
                return NotFound();
            }

            return View(media);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit (Media media)
        {
            var existingMedia = await _context.Media.FirstOrDefaultAsync(m => m.Id == media.Id);

            if (existingMedia == null)
            {
                return NotFound();
            }

            existingMedia.Type = media.Type;
            existingMedia.Title = media.Title;
            existingMedia.ReleaseDate = media.ReleaseDate;
            existingMedia.BoxOffice = media.BoxOffice;
            existingMedia.Duration = media.Duration;
            existingMedia.Description = media.Description;
            existingMedia.ImageUrl = media.ImageUrl;
            existingMedia.TrailerUrl = media.TrailerUrl;
            existingMedia.Director = media.Director;
            media.Phase = media.Phase;
            existingMedia.Saga = media.Saga;
            existingMedia.Chronology = media.Chronology;
            existingMedia.Studio = media.Studio;
            existingMedia.MultiverseDesignation = media.MultiverseDesignation;
            existingMedia.IsMCU = media.IsMCU;
            existingMedia.Season = media.Season;
            existingMedia.Episodes = media.Episodes;
            existingMedia.IsVisible = media.IsVisible;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = media.Id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete (int id)
        {
            var media = await _context.Media.FirstOrDefaultAsync(m => m.Id == id);

            return View(media);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete (Media media)
        {
            var existingMedia = await _context.Media.FirstOrDefaultAsync(m => m.Id == media.Id);

            if (existingMedia == null)
            {
                return NotFound();
            }

            _context.Media.Remove(existingMedia);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create ()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create (Media media)
        {
            _context.Media.Add(media);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = media.Id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportMovies()
        {
            for (int id = 1; id <= 75; id++)
            {
                var media = await _apiService.GetMediaAsync(id, "movies");

                if (media != null)
                {
                    var exists = await _context.Media.AnyAsync(m => m.Title == media.Title);

                    if (exists == false)
                    {
                        _context.Media.Add(media);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok("import complete");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportShows()
        {
            for (int id = 1; id <= 56; id++)
            {
                var media = await _apiService.GetMediaAsync(id, "tvshows");

                if (media != null)
                {
                    var exists = await _context.Media.AnyAsync(m => m.Title == media.Title && m.Season == media.Season);

                    if (exists == false)
                    {
                        _context.Media.Add(media);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok("import complete");
        }
    }
}
