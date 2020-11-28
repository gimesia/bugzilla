using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bugzilla.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // GET
        public async Task<IActionResult> Table(string? fixerName, string? reviewerName)
        {
            if (fixerName == null && reviewerName == null)
            {
                ViewData["reviews"] =
                    await _context
                        .Reviews
                        .Include(review => review.Dev)
                        .Include(review => review.Fix)
                        .ToListAsync();

                return View(await _context.Reviews.ToListAsync());
            }
            else
            {
                if (fixerName != "*" && reviewerName != "*")
                {
                    ViewData["reviews"] =
                        await _context
                            .Reviews
                            .Where(review => review.Dev.Name.Equals(reviewerName))
                            .Where(review => review.Fix.Dev.Name.Equals(fixerName))
                            .Include(review => review.Dev)
                            .Include(review => review.Fix)
                            .ToListAsync();

                    return View(await _context.Reviews.ToListAsync());
                }

                else if (fixerName != "*" && reviewerName == "*")
                {
                    ViewData["reviews"] =
                        await _context
                            .Reviews
                            .Where(review => review.Fix.Dev.Name.Equals(fixerName))
                            .Include(review => review.Dev)
                            .Include(review => review.Fix)
                            .ToListAsync();

                    return View(await _context.Reviews.ToListAsync());
                }

                else if (fixerName == "*" && reviewerName != "*")
                {
                    ViewData["reviews"] =
                        await _context
                            .Reviews
                            .Where(review => review.Dev.Name.Equals(reviewerName))
                            .Include(review => review.Dev)
                            .Include(review => review.Fix)
                            .ToListAsync();

                    return View(await _context.Reviews.ToListAsync());
                }
            }

            ViewData["reviews"] =
                await _context
                    .Reviews
                    .Include(review => review.Dev)
                    .Include(review => review.Fix)
                    .ToListAsync();

            return View(await _context.Reviews.ToListAsync());
        }

        public async Task<IActionResult> AddOrEdit(Guid? guid)
        {
            ViewData["devLeads"] = await _context.Developers .Include(i=>i.Role).Where(developer => developer.Role.Name == "Dev Lead")
               .ToListAsync();
            ViewData["fixes"] = await _context.Fixes.Include(i=>i.Bug).Where(i => _context.Reviews.Select(i=>i.Fix.Id).Contains(i.Id)).ToListAsync();
            if (guid != null) return View(await _context.Reviews.FirstOrDefaultAsync(i => i.Id == guid));
            var rev = new Review {Id = Guid.NewGuid(), Approved = false, Dev = null, Fix = null};
            return View(rev);
        }

        public async Task<IActionResult> AddOrEditDb(Guid id, Guid dev, Guid fix, bool approved)
        {
            var reviewToUpdate = await _context.Reviews.FindAsync(id);
            var developer = await _context.Developers.FindAsync(dev);
            var fixxer = await _context.Fixes.FindAsync(fix); 
            if (reviewToUpdate == null)
            {
                var review = new Review
                {
                    Approved = approved, Dev = developer, Fix = fixxer, Id = id
                };
                await _context.Reviews.AddAsync(review);
            }
            else
            {
                reviewToUpdate.Approved = approved;
                reviewToUpdate.Dev = developer;
                reviewToUpdate.Fix = fixxer;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid guid)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(i => i.Id == guid);
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

        public IActionResult Filter(string fixerName, string reviewerName)
        {
            return RedirectToAction("Table");
        }
    }
}