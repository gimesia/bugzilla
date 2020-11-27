using System;
using System.Linq;
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

        public IActionResult Index()
        {
            return RedirectToAction("Table");
        }        
        
        public async Task<IActionResult> Table(string? devName, string? bugDesc)
        {
            ViewData["fixes"]= await _context.Fixes.Include(fix => fix.Bug).Include(fix => fix.Dev).ToListAsync();
            return View(await _context.Fixes.ToListAsync());
        }

        public async Task<IActionResult> Add()
        {
            ViewData["fixes"]= await _context.Fixes.Include(fix => fix.Bug).Include(fix => fix.Dev).ToListAsync();
            return View();
        }  public async Task<IActionResult> Delete(Guid guid)
        {
            return RedirectToAction("Index");
        }
    }
}