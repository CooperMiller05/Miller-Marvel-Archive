using MarvelArchive.Data;
using MarvelArchive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarvelArchive.Controllers
{
    public class ComicSeriesController : Controller
    {
        private readonly MarvelDbContext _context;
        public ComicSeriesController(MarvelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportComicSeries()
        {
            for (int id = 1; id <= 731; id++)
            {
                var comicSeries = await _superheroApiService.GetCharacterAsync(id);

                if (comicSeries != null)
                {
                    var exists = await _context.ComicSeries.AnyAsync(c => c.Title == comicSeries.Title);

                    if (exists == false)
                    {
                        _context.ComicSeries.Add(comicSeries);
                    }
                }
            }
            await _context.SaveChangesAsync();

            return Ok("import complete");
        }
    }
}
