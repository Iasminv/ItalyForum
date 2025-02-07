using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ItalyForum.Data;
using ItalyForum.Models;

namespace ItalyForum.Controllers
{
    public class DiscussionsController : Controller
    {
        private readonly ItalyForumContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DiscussionsController(ItalyForumContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Discussions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Discussion.ToListAsync());
        }

        // GET: Discussions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // GET: Discussions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discussions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscussionId,Title,Content,ImageFile,CreateDate")] Discussion discussion)
        {
            if (ModelState.IsValid)
            {
                // Rename the uploaded file to a guid (unique filename)
                if (discussion.ImageFile != null)
                {
                    discussion.ImageFilename = Guid.NewGuid().ToString() + Path.GetExtension(discussion.ImageFile.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", discussion.ImageFilename);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await discussion.ImageFile.CopyToAsync(fileStream);
                    }
                }

                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }



        // GET: Discussions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion == null)
            {
                return NotFound();
            }
            return View(discussion);
        }

        // POST: Discussions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,ImageFile,ImageFilename,CreateDate")] Discussion discussion)
        {
            if (id != discussion.DiscussionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (discussion.ImageFile != null)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + discussion.ImageFile.FileName;
                        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        var filePath = Path.Combine(uploads, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await discussion.ImageFile.CopyToAsync(fileStream);
                        }

                        // Delete the old image file if exists
                        if (!string.IsNullOrEmpty(discussion.ImageFilename))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", discussion.ImageFilename);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        discussion.ImageFilename = uniqueFileName;
                    }

                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionExists(discussion.DiscussionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }


        // GET: Discussions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion != null)
            {
                // Delete the image file
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", discussion.ImageFilename);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _context.Discussion.Remove(discussion);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussion.Any(e => e.DiscussionId == id);
        }
    }
}
