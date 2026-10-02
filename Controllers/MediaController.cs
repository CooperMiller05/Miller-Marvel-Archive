using MarvelArchive.Data;
using MarvelArchive.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarvelArchive.Controllers
{
    public class MediaController : Controller
    {
        private readonly MCUTimelineApiService _apiService;
        private readonly MarvelDbContext _context;

        public MediaController(MCUTimelineApiService apiService, MarvelDbContext context)
        {
            _apiService = apiService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ImportCharacters()
        {
            for (int id = 1; id <= 731; id++)
            {
                var character = await _apiService.GetCharacterAsync(id);

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
