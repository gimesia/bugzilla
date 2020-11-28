using System;
using System.Collections;
using System.Collections.Generic;
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

        public async Task<IActionResult> Table(Guid? fixer, Guid? bug)
        {
            ViewData["fixes"] = await _context.Fixes
                .Include(fix => fix.Bug)
                .Include(fix => fix.Bug.Dev)
                .Include(fix => fix.Dev).ToListAsync();
            if (fixer == null || bug == null)
                return View(await _context.Fixes.ToListAsync());

            var fixes = (IEnumerable<Fix>) await _context.Fixes.ToListAsync();

            fixes = bug == Guid.Empty ? fixes : fixes.Where(fix => fix.Bug.Id == bug);
            fixes = fixer == Guid.Empty ? fixes : fixes.Where(fix => fix.Dev.Id == fixer);

            return View(fixes);
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
            var fix = new Fix {Bug = bugger, Dev = developer, Id = id};

            _context.Fixes.Add(fix);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var fix = await _context.Fixes.FindAsync(id);
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