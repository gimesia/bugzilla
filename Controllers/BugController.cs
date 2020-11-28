using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using bugzilla.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction("Table");
        }

        public async Task<IActionResult> Table(Guid? dev, string? closed)
        {
            ViewData["bugs"] = await _context.Bugs.Include(bug => bug.Dev).ToListAsync();
            if (dev == null || closed == null)
                return View(await _context.Bugs.ToListAsync());

            var bugs = (IEnumerable<Bug>) await _context.Bugs.ToListAsync();
            if (dev != Guid.Empty)
            {
                bugs = bugs.Where(bug => bug.Dev.Id == dev);
            }

            switch (closed)
            {
                case "open":
                {
                    bugs = bugs.Where(bug => bug.Closed == false);
                    break;
                }
                case "closed":
                {
                    bugs = bugs.Where(bug => bug.Closed == true);
                    break;
                }
                    default:break;
            }

            return View(bugs);
        }

        public async Task<IActionResult> AddOrEdit(Guid? guid)
        {
            ViewData["bug"] = await _context.Bugs.FirstOrDefaultAsync(i => i.Id == guid);
            ViewData["devs"] = await _context.Developers.ToListAsync();
            return guid != null
                ? View(await _context.Bugs.FirstOrDefaultAsync(i => i.Id == guid))
                : View(new Bug {Id = Guid.NewGuid(), Description = "", Closed = false, Dev = null});
        }


        public async Task<IActionResult> Delete(Guid guid)
        {
            var bug = await _context.Bugs.FirstOrDefaultAsync(i => i.Id == guid);
            if (bug == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                _context.Bugs.Remove(bug);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> AddOrEditDb(Guid id, Guid dev, string description, bool closed)
        {
            var bugToUpdate = await _context.Bugs.FindAsync(id);
            var developer = await _context.Developers.FindAsync(dev);
            if (bugToUpdate == null)
            {
                bugToUpdate = new Bug {Id = id, Description = description, Dev = developer, Closed = closed};
                await _context.Bugs.AddAsync(bugToUpdate);
            }
            else
            {
                bugToUpdate.Description = description;
                bugToUpdate.Dev = developer;
                bugToUpdate.Closed = closed;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}