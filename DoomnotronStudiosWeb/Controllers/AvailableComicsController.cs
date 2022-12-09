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
    public class AvailableComicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvailableComicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AvailableComics
        public async Task<IActionResult> Index()
        {
              return View(await _context.Comics.ToListAsync());
        }

        // GET: AvailableComics/Details/5
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
    }
}
