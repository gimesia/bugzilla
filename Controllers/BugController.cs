using System;
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

        public async Task<IActionResult> Table()
        {
            ViewData["bugs"] = await _context.Bugs.Include(bug => bug.Dev).ToListAsync();
            return View(await _context.Bugs.ToListAsync());
        }

        public async Task<IActionResult> AddOrEdit(Guid? guid)
        {
            //TODO bug models description doesnt show
            ViewData["bug"] = await _context.Bugs.FirstOrDefaultAsync(i => i.Id == guid);
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
        /*public async Task<IActionResult> AddOrEditDb(Guid id, string description)
        {
            var boh = Request.Cookies["DevId.Cookie"];
            var bugToUpdate = await _context.Bugs.FindAsync(id);

            if (bugToUpdate == null)
            {
                
                bugToUpdate = new Bug{Id = id, Description = description, Dev = await _context.Developers.FindAsync()};
                await _context.Developers.AddAsync(bugToUpdate);
            }
            else
            {
                bugToUpdate.Name = name;
                bugToUpdate.Role = await _context.Roles.FindAsync(role);
            }
            
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }*/

    }
}