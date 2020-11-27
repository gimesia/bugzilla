using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using bugzilla.Models;
using Microsoft.EntityFrameworkCore;

namespace bugzilla.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BugzillaDbContext _context;
        
        public HomeController(ILogger<HomeController> logger, BugzillaDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["devs"] =
                await _context
                    .Developers
                    .Include(review => review.Role)
                    .ToListAsync();

            return View(await _context.Developers.ToListAsync());
        }

        public IActionResult Login(string name)
        {
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}