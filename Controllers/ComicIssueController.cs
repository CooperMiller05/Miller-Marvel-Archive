using MarvelArchive.Data;
using Microsoft.AspNetCore.Mvc;

namespace MarvelArchive.Controllers
{
    public class ComicIssueController : Controller
    {
        private readonly MarvelDbContext _context;

        public ComicIssueController(MarvelDbContext context)
        {
            _context = context;
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
