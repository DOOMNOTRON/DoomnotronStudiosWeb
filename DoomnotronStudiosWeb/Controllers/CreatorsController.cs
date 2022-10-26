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
    public class CreatorsController : Controller
    {
        private readonly ICreatorRepository _creatorRepo;

        public CreatorsController(ICreatorRepository creatorRepo)
        {
            _creatorRepo = creatorRepo;
        }

        // GET: Creators
        public async Task<IActionResult> Index()
        {
              return View(await _creatorRepo.GetAllCreators());
        }

        // GET: Creators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creator = await _creatorRepo.GetCreator(id.Value);

            if (creator == null)
            {
                return NotFound();
            }

            return View(creator);
        }

        // GET: Creators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Creators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName")] Creator creator)
        {
            if (ModelState.IsValid)
            {
                await _creatorRepo.SaveCreator(creator);
                return RedirectToAction(nameof(Index));
            }
            return View(creator);
        }

        // GET: Creators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creator = await _creatorRepo.GetCreator(id.Value);
            if (creator == null)
            {
                return NotFound();
            }
            return View(creator);
        }

        // POST: Creators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName")] Creator creator)
        {
            if (id != creator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _creatorRepo.UpdateCreator(creator);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CreatorExists(creator.Id))
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
            return View(creator);
        }

        // GET: Creators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creator = await _creatorRepo.GetCreator(id.Value);
            if (creator == null)
            {
                return NotFound();
            }

            return View(creator);
        }

        // POST: Creators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var creator = await _creatorRepo.GetCreator(id);

            TempData["Message"] = $"{creator.FullName} was removed from any related comics";

            // Remove creator
            await _creatorRepo.DeleteCreator(creator.Id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CreatorExists(int id)
        {
          return await _creatorRepo.GetCreator(id) != null;
        }
    }
}
