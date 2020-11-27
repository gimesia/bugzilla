using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bugzilla.Models;
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
            return View(guid != null
                ? await _context.Reviews.FindAsync(guid)
                : new Review {Id = Guid.NewGuid(), Fix = null, Dev = null});
        }

        public IActionResult Delete(Guid guid)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Filter(string fixerName, string reviewerName)
        {
            return RedirectToAction("Table");
        }
    }
}