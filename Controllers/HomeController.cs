using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using bugzilla.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace bugzilla.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BugzillaDbContext _context;

        const string SessionId = "_Id";

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

        public async Task<IActionResult> Login(Guid id)
        {
            var developer = await _context.Developers.FindAsync(id);
            if (developer == null)
            {
                return RedirectToAction("Index");
            }
            
            var role = _context.Roles
                .Where(i => i.Id == _context.Developers
                .Find(id).Role.Id)
                .Select(i => i.Name)
                .ToString();
            
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, developer.Name),
                    new Claim(ClaimTypes.UserData, developer.Id.ToString()),
                    new Claim(ClaimTypes.Role, role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Claims Identity");

                var userPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index", "Developer");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public async Task<IActionResult> Secret(Guid devId)
        {
            var developer = await _context.Developers.FindAsync(devId);
          

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Authenticate(Guid devId)
        {
            var developer = await _context.Developers.FindAsync(devId);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, developer.Name),
                new Claim(ClaimTypes.UserData, developer.Id.ToString()),
                new Claim(ClaimTypes.Role, developer.Role.Id.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Claims Identity");

            var userPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(userPrincipal);
            return RedirectToAction("Index");
        }
    }
}