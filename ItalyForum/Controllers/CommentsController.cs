using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ItalyForum.Data;
using ItalyForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ItalyForum.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ItalyForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ItalyForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Comments/Create
        // Update the GET method to accept ID from route
        public IActionResult Create(int id)  // Changed from discussionId to id
        {
            ViewBag.DiscussionId = id;
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Content,DiscussionId,ApplicationUserId")] Comment comment)
        {
            // If ApplicationUserId wasn't set in the form, set it here
            if (string.IsNullOrEmpty(comment.ApplicationUserId))
            {
                comment.ApplicationUserId = _userManager.GetUserId(User);
            }

            if (ModelState.IsValid)
            {
                comment.CreateDate = DateTime.Now;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetDiscussion", "Home", new { id = comment.DiscussionId });
            }

            ViewBag.DiscussionId = comment.DiscussionId;
            return View(comment);
        }
    }
}