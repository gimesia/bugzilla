using System;
using System.Linq;
using System.Threading.Tasks;
using bugzilla.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Digests;

namespace bugzilla.Controllers
{
    public class DeveloperController : Controller
    {
        private readonly BugzillaDbContext _context;

        public DeveloperController(BugzillaDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction("Table");
        }

        public async Task<IActionResult> Table(Guid? role)
        {
            ViewData["devs"] = await _context.Developers.Include(dev => dev.Role).ToListAsync();
            return role == null
                ? View(await _context.Developers.ToListAsync())
                : View(await _context.Developers.Where(developer => developer.Role.Id == role).ToListAsync());
        }

        public async Task<IActionResult> AddOrEdit(Guid? guid)
        {
            ViewData["roles"] = await _context.Roles.ToListAsync();
            if (guid != null) return View(await _context.Developers.FirstOrDefaultAsync(i => i.Id == guid));
            var dev = new Developer {Id = Guid.NewGuid(), Name = "", Role = null};
            return View(dev);
        }

        public async Task<IActionResult> Delete(Guid guid)
        {
            var developer = await _context.Developers.FirstOrDefaultAsync(i => i.Id == guid);
            if (developer == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                _context.Developers.Remove(developer);
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

        public async Task<IActionResult> AddOrEditDb(Guid id, string name, Guid role)
        {
            var developerToUpdate = await _context.Developers.FindAsync(id);

            if (developerToUpdate == null)
            {
                developerToUpdate = new Developer {Id = id, Name = name, Role = await _context.Roles.FindAsync(role)};
                await _context.Developers.AddAsync(developerToUpdate);
            }
            else
            {
                developerToUpdate.Name = name;
                developerToUpdate.Role = await _context.Roles.FindAsync(role);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}