using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazordCrud.Data;
using RazordCrud.Models;

namespace RazordCrud.Controllers
{
    public class MusicsController : Controller
    {
        private readonly MvcMovieContext _context;

        public MusicsController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Musics
        public async Task<IActionResult> Index()
        {
            return View(await _context.Musics.ToListAsync());
        }

        // GET: Musics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musics = await _context.Musics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (musics == null)
            {
                return NotFound();
            }

            return View(musics);
        }

        // GET: Musics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Musics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price")] Musics musics)
        {
            if (ModelState.IsValid)
            {
                _context.Add(musics);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(musics);
        }

        // GET: Musics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musics = await _context.Musics.FindAsync(id);
            if (musics == null)
            {
                return NotFound();
            }
            return View(musics);
        }

        // POST: Musics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price")] Musics musics)
        {
            if (id != musics.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(musics);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicsExists(musics.Id))
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
            return View(musics);
        }

        // GET: Musics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musics = await _context.Musics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (musics == null)
            {
                return NotFound();
            }

            return View(musics);
        }

        // POST: Musics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musics = await _context.Musics.FindAsync(id);
            _context.Musics.Remove(musics);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MusicsExists(int id)
        {
            return _context.Musics.Any(e => e.Id == id);
        }
    }
}
