using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ItalyForum.Models;
using ItalyForum.Data;

namespace ItalyForum.Controllers
{
    public class HomeController : Controller
    {

        private readonly ItalyForumContext _context;

        //Constructor
        public HomeController(ItalyForumContext context)
        {
            _context = context;
        }

        // 
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Discussion> discussions = await _context.Discussion.ToListAsync();
                return View(discussions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "An error occurred while fetching the discussion posts. Please try again later.";
                return View(new List<Discussion>());
            }
        }

        // Discussion Details - Display discussion posts

        public async Task<IActionResult> DiscussionDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion.FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
