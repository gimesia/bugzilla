using System;
using System.Linq;
using System.Threading.Tasks;
using bugzilla.Models;
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

        public IActionResult Index()
        {
            return RedirectToAction("Table");
        }

        public async Task<IActionResult> Table()
        {
            ViewData["devs"] = await _context.Developers.Include(dev => dev.Role).ToListAsync();
            return View(await _context.Developers.ToListAsync());
        }

        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            ViewData["roles"] = await _context.Roles.ToListAsync();
            if (id != Guid.Empty)
            {
                var dev = new Developer {Id = new Guid(), Name = "", Role = null};
                return View(dev);
            }
            else
                return View(await _context.Developers.FindAsync(id));
        }

        public async Task<IActionResult> Delete(Guid guid)
        {
            return RedirectToAction("Index");
        }

    }
}