using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSP2.Models;

namespace SSP2.Controllers;

    [Authorize]
    public class FoliosController : Controller
    {
        private readonly LabcompContext _context;

        public FoliosController(LabcompContext context)
        {
            _context = context;
        }

        // GET: Folios
        public async Task<IActionResult> Index()
        {
              return _context.Folios != null ? 
                          View(await _context.Folios.ToListAsync()) :
                          Problem("Entity set 'LabcompContext.Folios'  is null.");
        }

        // GET: Folios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Folios == null)
            {
                return NotFound();
            }

            var folio = await _context.Folios
                .FirstOrDefaultAsync(m => m.FolId == id);
            if (folio == null)
            {
                return NotFound();
            }

            return View(folio);
        }

        // GET: Folios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Folios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FolId,FolNumero,FolNu,FolFecha")] Folio folio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(folio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(folio);
        }

        // GET: Folios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Folios == null)
            {
                return NotFound();
            }

            var folio = await _context.Folios.FindAsync(id);
            if (folio == null)
            {
                return NotFound();
            }
            return View(folio);
        }

        // POST: Folios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FolId,FolNumero,FolNu,FolFecha")] Folio folio)
        {
            if (id != folio.FolId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(folio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FolioExists(folio.FolId))
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
            return View(folio);
        }

        // GET: Folios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Folios == null)
            {
                return NotFound();
            }

            var folio = await _context.Folios
                .FirstOrDefaultAsync(m => m.FolId == id);
            if (folio == null)
            {
                return NotFound();
            }

            return View(folio);
        }

        // POST: Folios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Folios == null)
            {
                return Problem("Entity set 'LabcompContext.Folios'  is null.");
            }
            var folio = await _context.Folios.FindAsync(id);
            if (folio != null)
            {
                _context.Folios.Remove(folio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FolioExists(int id)
        {
          return (_context.Folios?.Any(e => e.FolId == id)).GetValueOrDefault();
        }
    }
