using System;
using System.Linq;
using System.Threading.Tasks;
using bugzilla.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction("Table");
        }

        public async Task<IActionResult> Table(string? devName, string? bugDesc)
        {
            ViewData["fixes"] = await _context.Fixes.Include(fix => fix.Bug).Include(fix => fix.Dev).ToListAsync();
            return View(await _context.Fixes.ToListAsync());
        }

        public async Task<IActionResult> Add()
        {
            ViewData["fixes"] = await _context.Fixes.Include(fix => fix.Bug).Include(fix => fix.Dev).ToListAsync();
            ViewData["devs"] = await _context.Developers.ToListAsync();
            ViewData["bugs"] = await _context.Bugs.ToListAsync();
            return View();
        }

        public async Task<IActionResult> AddDb(Guid id, Guid dev, Guid bug)
        {
            var developer = await _context.Developers.FindAsync(dev);
            var bugger = await _context.Bugs.FindAsync(bug);
            var fix = new Fix { Bug = bugger, Dev = developer, Id = id};

            _context.Fixes.Add(fix);
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid guid)
        {
            var fix = await _context.Fixes.FirstOrDefaultAsync(i => i.Id == guid);
            if (fix == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                _context.Fixes.Remove(fix);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException e)
            {
                //Log the error (uncomment ex variable name and write a log.)
                Console.WriteLine(e);
                return RedirectToAction("Index");
            }
        }
    }
}