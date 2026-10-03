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

        public IActionResult Index()
        {
            return View();
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
