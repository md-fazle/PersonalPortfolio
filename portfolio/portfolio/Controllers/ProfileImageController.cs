using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portfolio.Context;
using portfolio.Models;
using System.IO;
using System.Threading.Tasks;

namespace portfolio.Controllers
{
    public class ProfileImageController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileImageController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /ProfileImage
        public async Task<IActionResult> Index()
        {
            var images = await _context.profileimg.ToListAsync();
            return View(images);
        }

        // GET: /ProfileImage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var image = await _context.profileimg.FirstOrDefaultAsync(m => m.Id == id);
            if (image == null) return NotFound();

            return View(image);
        }

        // GET: /ProfileImage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /ProfileImage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title")] ProfileImage profileImage, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/Images", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    profileImage.ImgPath = "/Images/" + file.FileName;
                    _context.Add(profileImage);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("File", "Please upload an image file.");
            }
            return View(profileImage);
        }

        // GET: /ProfileImage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var image = await _context.profileimg.FindAsync(id);
            if (image == null) return NotFound();

            return View(image);
        }

        // POST: /ProfileImage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ImgPath")] ProfileImage profileImage)
        {
            if (id != profileImage.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profileImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(profileImage.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(profileImage);
        }

        //GET: /ProfileImage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var image = await _context.profileimg.FirstOrDefaultAsync(m => m.Id == id);
            if (image == null) return NotFound();

            return View(image);
        }

        // POST: /ProfileImage/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.profileimg.FindAsync(id);
            if (image != null)
            {
                // Delete the image file from the server
                var filePath = Path.Combine("wwwroot/images", image.ImgPath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Remove the image from the database
                _context.profileimg.Remove(image);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index"); // Redirect to Index after deletion
        }
        private bool ImageExists(int id)
        {
            return _context.profileimg.Any(e => e.Id == id);
        }
    }
}
