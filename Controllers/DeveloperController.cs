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

        // GET
        public async Task<IActionResult> Index()
        {
            ViewData["roles"] = await _context.Roles.ToListAsync();
            return View(await _context.Developers.ToListAsync());
        }

        public async Task<IActionResult> AddOrEdit(Guid? id)
        {
            ViewData["roles"] = await _context.Roles.ToListAsync();
            if (id != null)
            {
                var dev = new Developer {Id = new Guid(), Name = "", Role = null};
                return View(dev);
            }
            else
                return View(_context.Developers.Find(id));
        }
    }
}