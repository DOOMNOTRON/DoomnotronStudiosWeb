using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoomnotronStudiosWeb.Data;
using DoomnotronStudiosWeb.Models;

namespace DoomnotronStudiosWeb.Controllers
{
    public class ComicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comics
        public async Task<IActionResult> Index()
        {
            List<ComicIndexViewModel> ComicData = await (from c in _context.Comics
                            join creator in _context.Creators
                                on c.ComicCreator.Id equals creator.Id
                            orderby c.Title
                            select new ComicIndexViewModel
                            {
                                ComicId = c.Id,
                                ComicTitle = c.Title,
                                ComicCreatorName = creator.FullName
                            }).ToListAsync();

              return View(ComicData);
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
                Comic newComic = new()
                {
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
                    if (!ComicExists(comic.Id))
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

        private bool ComicExists(int id)
        {
          return _context.Comics.Any(e => e.Id == id);
        }
    }
}
