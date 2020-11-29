using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bugzilla.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Digests;

namespace bugzilla.Controllers
{
    public class ReviewController : Controller
    {
        private readonly BugzillaDbContext _context;

        public ReviewController(BugzillaDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction("Table", "Review");
        }

        public async Task<IActionResult> Table(Guid? reviewer, Guid? fixer, string? approved)
        {
            ViewData["reviews"] = await _context.Reviews
                .Include(i => i.Dev)
                .Include(i => i.Dev.Role)
                .Include(i => i.Fix)
                .Include(i => i.Fix.Dev)
                .Include(i => i.Fix.Dev.Role)
                .Include(i => i.Fix.Bug)
                .ToListAsync();
            if (reviewer == null || fixer == null || approved == null)
                return View(await _context.Reviews.ToListAsync());

            var reviews = (IEnumerable<Review>) await _context.Reviews
                .Include(i => i.Dev)
                .Include(i => i.Dev.Role)
                .Include(i => i.Fix)
                .Include(i => i.Fix.Dev)
                .Include(i => i.Fix.Dev.Role)
                .Include(i => i.Fix.Bug)
                .ToListAsync();

            reviews = reviewer == Guid.Empty
                ? reviews
                : reviews.Where(review => review.Dev.Id == reviewer);

            reviews = fixer == Guid.Empty
                ? reviews
                : reviews.Where(review => review.Fix.Dev.Id == fixer);

            switch (approved)
            {
                case "approved":
                {
                    reviews = reviews.Where(review => review.Approved == true);
                    break;
                }
                case "rejected":
                {
                    reviews = reviews.Where(review => review.Approved == false);
                    break;
                }
                default: break;
            }

            return View(reviews);
        }

        public async Task<IActionResult> AddOrEdit(Guid? id)
        {
            ViewData["devLeads"] = await _context.Developers
                .Include(i => i.Role)
                .Where(developer => developer.Role.Name == "Dev Lead")
                .ToListAsync();

            ViewData["fixes"] = await _context.Fixes
                .Include(i=>i.Bug)
                .Include(i=>i.Bug.Dev)
                .Where(i => _context.Reviews
                    .Include(item => item.Fix)
                    .Include(item => item.Fix.Dev)
                    .Select(item => item.Fix.Id)
                    .Contains(i.Id) == false)
                .ToListAsync();

            if (id != null)
                return View(await _context.Reviews
                    .FirstOrDefaultAsync(i => i.Id == id));

            var rev = new Review
            {
                Id = Guid.NewGuid(),
                Approved = false,
                Dev = null,
                Fix = null
            };
            
            return View(rev);
        }

        public async Task<IActionResult> AddOrEditDb(Guid id, Guid dev, Guid fix, bool approved)
        {
            var reviewToUpdate = await _context.Reviews.FindAsync(id);
            var developer = await _context.Developers.FindAsync(dev);
            var fixx = await _context.Fixes.FindAsync(fix);
         
            if (reviewToUpdate == null)
            {
                var review = new Review
                {
                    Approved = approved, Dev = developer, Fix = fixx, Id = id
                };
                await _context.Reviews.AddAsync(review);
            }
            else
            {
                reviewToUpdate.Approved = approved;
                reviewToUpdate.Dev = developer;
                reviewToUpdate.Fix = fixx;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(i => i.Id == id);
            
            if (review == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("Index");
            }
        }
    }
}