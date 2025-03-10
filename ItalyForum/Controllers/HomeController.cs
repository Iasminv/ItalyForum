using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ItalyForum.Models;
using ItalyForum.Data;
using System.Linq;

namespace ItalyForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ItalyForumContext _context;

        // Constructor
        public HomeController(ItalyForumContext context)
        {
            _context = context;
        }

        // Home Page - Display all discussion threads in descending order by create date
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Discussion> discussions = await _context.Discussion
                    .Include(d => d.Comments)
                    .Include(d => d.ApplicationUser)
                    .OrderByDescending(d => d.CreateDate)
                    .ToListAsync();

                return View(discussions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "An error occurred while fetching the discussion posts. Please try again later.";
                return View(new List<Discussion>());
            }
        }

        // Get Discussion - Display a specific discussion's details
        public async Task<IActionResult> GetDiscussion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .Include(d => d.Comments)
                    .ThenInclude(c => c.ApplicationUser)  
                .Include(d => d.ApplicationUser) 
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // Display form to create a new comment
        public IActionResult CreateComment(int discussionId)
        {
            ViewBag.DiscussionId = discussionId;
            return View();
        }

        // Save the new comment
        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetDiscussion), new { id = comment.DiscussionId });
            }
            ViewBag.DiscussionId = comment.DiscussionId;
            return View(comment);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Display user profile
        public async Task<IActionResult> Profile(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // Get user's discussions
            var discussions = await _context.Discussion
                .Where(d => d.ApplicationUserId == id)
                .Include(d => d.Comments)
                .OrderByDescending(d => d.CreateDate)
                .ToListAsync();

            ViewBag.User = user;
            return View(discussions);
        }
    }
}
