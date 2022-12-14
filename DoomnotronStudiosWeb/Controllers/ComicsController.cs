using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoomnotronStudiosWeb.Data;
using DoomnotronStudiosWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace DoomnotronStudiosWeb.Controllers
{
    [Authorize(Roles = IdentityHelper.Creator)]
    public class ComicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ComicsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Comics
        public async Task<IActionResult> Index()
        {
              return View(await _context.Comics.ToListAsync());
        }

        // GET: Comics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comics == null)
            {
                return NotFound();
            }

            var comic = await _context.Comics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        // GET: Comics/Create
        public IActionResult Create()
        {
            ComicCreateViewModel viewModel = new();
            viewModel.AllAvailableComicCreators = _context.Creators.OrderBy(i => i.FullName).ToList();
            return View(viewModel);
        }

        // POST: Comics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComicCreateViewModel comic)
        {
            if (ModelState.IsValid)
            {
                string fileName = Guid.NewGuid().ToString();
                fileName += Path.GetExtension(comic.ProductPhoto.FileName);

                // Save file to the file system
                string uploadPath = Path.Combine(_environment.WebRootPath, "images",fileName);
                using Stream fileStream = new FileStream(uploadPath, FileMode.Create);
                await comic.ProductPhoto.CopyToAsync(fileStream);

                // Map our viewmodel to our data model (Comic), save to DB
                Comic newComic = new()
                {
                    PhotoUrl= fileName,
                    Title = comic.Title,
                    Artist = comic.Artist,
                    Writer = comic.Writer,
                    Description = comic.Description,
                    rating = comic.rating,
                    ArcNumber = comic.ArcNumber,
                    IssueNumber = comic.IssueNumber,
                    ComicCreator = new Creator()
                    {
                        Id = comic.ChosenCreator
                    }

                };

                // Tell EF that we have not modified the existing instructor
                _context.Entry(newComic.ComicCreator).State = EntityState.Unchanged;
                _context.Add(newComic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            comic.AllAvailableComicCreators = _context.Creators.OrderBy(i => i.FullName).ToList();
            return View(comic);
        }

        // GET: Comics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comics == null)
            {
                return NotFound();
            }

            var comic = await _context.Comics.FindAsync(id);
            if (comic == null)
            {
                return NotFound();
            }
            return View(comic);
        }

        // POST: Comics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Artist,Writer,Description,rating,ArcNumber,IssueNumber")] Comic comic)
        {
            if (id != comic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ComicExists(comic.Id))
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
            return View(comic);
        }

        // GET: Comics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comics == null)
            {
                return NotFound();
            }

            var comic = await _context.Comics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        // POST: Comics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comics == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Comics'  is null.");
            }
            var comic = await _context.Comics.FindAsync(id);
            if (comic != null)
            {
                _context.Comics.Remove(comic);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<bool> ComicExists(int id)
        {
            return await _context.Comics.AnyAsync(e => e.Id == id);
        }
    }
}
