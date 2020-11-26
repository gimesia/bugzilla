using System.Threading.Tasks;
using bugzilla.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bugzilla.Controllers
{
    public class ReviewController : Controller
    {
        private readonly BugzillaDbContext _context;

        public ReviewController(BugzillaDbContext context)
        {
            _context = context;
        }

        // GET
        public async Task<IActionResult> Index()
        {
            ViewData["devs"] = await _context.Developers.ToListAsync();
            ViewData["fixes"] = await _context.Fixes.ToListAsync();
            return View(await _context.Reviews.ToListAsync());
        }
    }
}