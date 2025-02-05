using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ItalyForum.Data;
using ItalyForum.Models;

namespace ItalyForum.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ItalyForumContext _context;

        public CommentsController(ItalyForumContext context)
        {
            _context = context;
        }

        // GET: Comments/Create
        public IActionResult Create(int discussionId)
        {
            ViewBag.DiscussionId = discussionId;
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Content,CreateDate,DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetDiscussion", "Home", new { id = comment.DiscussionId });
            }
            ViewBag.DiscussionId = comment.DiscussionId;
            return View(comment);
        }
    }
}
