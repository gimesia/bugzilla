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
            ViewData["fixes"] = await _context.Fixes
                .Where(fix => _context.Reviews.Select(i => i.Fix.Id).Contains(fix.Id)).ToListAsync();
            if (guid != null) return View(await _context.Reviews.FirstOrDefaultAsync(i => i.Id == guid));
            var rev = new Review {Id = Guid.NewGuid(), Approved = false, Dev = null, Fix = null};
            return View(rev);
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
                //Log the error (uncomment ex variable name and write a log.)
                Console.WriteLine(e);
                return RedirectToAction("Index");
            }
        }

        public IActionResult Filter(string fixerName, string reviewerName)
        {
            return RedirectToAction("Table");
        }
    }
}