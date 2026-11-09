using MarvelArchive.Data;
using MarvelArchive.Models;
using MarvelArchive.Services;
using MarvelArchive.ViewModels;
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

        public async Task<IActionResult> Index(string? search, string type = "all", string sort = "atoz", int page = 1)
        {
            const int pageSize = 25;

            var allMedia = await _context.Media.AsNoTracking().Select(m => new MediaViewModel
            {
                Id = m.Id,
                Type = m.Type,
                Title = m.Title,
                Description = m.Description,
                IsVisible = m.IsVisible
            }).ToListAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                allMedia = allMedia.Where(m =>
                        (m.Title?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (m.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
            }

            if (type == "movie")
            {
                allMedia = allMedia.Where(m => m.Type == "movie").ToList();
            }
            else if (type == "tvshow")
            {
                allMedia = allMedia.Where(c => c.Type == "tvshow").ToList();
            }

            if (sort == "ztoa")
            {
                allMedia = allMedia.OrderByDescending(m => m.Title).ToList();
            }
            else
            {
                allMedia = allMedia.OrderBy(m => m.Title).ToList();
            }

            int totalMedia = allMedia.Count;

            int totalPages = (int)Math.Ceiling(totalMedia / (double)pageSize);

            page = Math.Max(1, page);
            page = totalPages > 0 ? Math.Min(page, totalPages) : 1;

            var media = allMedia.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Search = search;
            ViewBag.Type = type;
            ViewBag.Sort = sort;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(media);
        }

        public async Task<IActionResult> SearchSuggestions(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
            {
                return Json(new List<object>());
            }

            var suggestions = await _context.Media.AsNoTracking().Where(m => m.IsVisible && m.Title.Contains(term)).OrderBy(m => m.Title).Take(5).Select(m => new
            {
                m.Id,
                m.Title,
                m.Type
            }).ToListAsync();

            return Json(suggestions);
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
        public async Task<IActionResult> Edit(Media media)
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
        public async Task<IActionResult> Delete(int id)
        {
            var media = await _context.Media.FirstOrDefaultAsync(m => m.Id == id);

            return View(media);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(Media media)
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
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Media media)
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
