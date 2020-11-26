using System.Threading.Tasks;
using bugzilla.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bugzilla.Controllers
{
    public class BugController : Controller
    {
        private readonly BugzillaDbContext _context;

        public BugController(BugzillaDbContext context)
        {
            _context = context;
        }

        // GET
        public async Task<IActionResult> Index()
        {
            ViewData["devs"] = await _context.Developers.ToListAsync();
            return View(await _context.Bugs.ToListAsync());
        }
    }
}