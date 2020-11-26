using System.Threading.Tasks;
using bugzilla.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bugzilla.Controllers
{
    public class FixController : Controller
    {
        private readonly BugzillaDbContext _context;

        public FixController(BugzillaDbContext context)
        {
            _context = context;
        }

        // GET
        public async Task<IActionResult> Index()
        {
            ViewData["devs"] = await _context.Developers.ToListAsync();
            ViewData["bugs"] = await _context.Bugs.ToListAsync();
            return View(await _context.Fixes.ToListAsync());
        }
    }
}