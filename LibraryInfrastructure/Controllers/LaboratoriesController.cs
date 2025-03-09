using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryDomain.Models;
using LibraryInfrastructure;

namespace LibraryInfrastructure.Controllers
{
    public class LaboratoriesController : Controller
    {
        private readonly DblibraryContext _context;

        public LaboratoriesController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Laboratories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Laboratories.ToListAsync());
        }

        // GET: Laboratories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laboratory = await _context.Laboratories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (laboratory == null)
            {
                return NotFound();
            }

            return View(laboratory);
        }

        // GET: Laboratories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Laboratories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabNumber,Id")] Laboratory laboratory)
        {
            // Check if a laboratory with the same LabNumber already exists
            if (_context.Laboratories.Any(l => l.LabNumber == laboratory.LabNumber))
            {
                ModelState.AddModelError("LabNumber", "Лабораторія з таким номером вже існує.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(laboratory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(laboratory);
        }

        // GET: Laboratories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laboratory = await _context.Laboratories.FindAsync(id);
            if (laboratory == null)
            {
                return NotFound();
            }
            return View(laboratory);
        }

        // POST: Laboratories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LabNumber,Id")] Laboratory laboratory)
        {
            if (id != laboratory.Id)
            {
                return NotFound();
            }

            // Check for duplicates, excluding the current laboratory record
            if (_context.Laboratories.Any(l => l.LabNumber == laboratory.LabNumber && l.Id != laboratory.Id))
            {
                ModelState.AddModelError("LabNumber", "Лабораторія з таким номером вже існує.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(laboratory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaboratoryExists(laboratory.Id))
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
            return View(laboratory);
        }

        // GET: Laboratories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laboratory = await _context.Laboratories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (laboratory == null)
            {
                return NotFound();
            }

            return View(laboratory);
        }

        // POST: Laboratories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var laboratory = await _context.Laboratories.FindAsync(id);
            if (laboratory == null)
            {
                return NotFound();
            }
            try
            {
                _context.Laboratories.Remove(laboratory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["DeleteError"] = "Неможливо видалити лабораторію, оскільки існують пов'язані записи.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LaboratoryExists(int id)
        {
            return _context.Laboratories.Any(e => e.Id == id);
        }
    }
}
