using System;
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

        public IActionResult Index()
        {
            return RedirectToAction("Table");
        }

        public async Task<IActionResult> Table()
        {
            ViewData["bugs"] = await _context.Bugs.Include(bug => bug.Dev).ToListAsync();
            return View(await _context.Bugs.ToListAsync());
        }

        public async Task<IActionResult> AddOrEdit(Guid? guid)
        {
            return View(guid!=null
                ? await _context.Bugs.FindAsync(guid)
                : new Bug {Id = Guid.NewGuid(), Closed = false, Description = "", /*TODO DEV*/Dev = null});
        }
        
        public async Task<IActionResult> Delete(Guid guid)
        {
            return RedirectToAction("Index");
        }
    }
}