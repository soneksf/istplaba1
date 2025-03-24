using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryDomain.Models;
using LibraryInfrastructure;

namespace LibraryInfrastructure.Controllers
{
    public class AreasController : Controller
    {
        private readonly DblibraryContext _context;

        public AreasController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Areas
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Areas.ToListAsync());
        }

        // GET: Areas/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var area = await _context.Areas.FirstOrDefaultAsync(m => m.Id == id);
            if (area == null) return NotFound();

            return View(area);
        }

        // GET: Areas/Create
        [Authorize] // лише авторизовані можуть створювати
        public IActionResult Create()
        {
            return View();
        }

        // POST: Areas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // лише авторизовані можуть створювати
        public async Task<IActionResult> Create([Bind("AreaName,Id")] Area area)
        {
            // Перевірка на дублікати
            if (_context.Areas.Any(a => a.AreaName == area.AreaName))
            {
                ModelState.AddModelError("AreaName", "Дослідна область з такою назвою вже існує.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(area);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(area);
        }

        // GET: Areas/Edit/5
        [Authorize] // лише авторизовані можуть редагувати
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var area = await _context.Areas.FindAsync(id);
            if (area == null) return NotFound();

            return View(area);
        }

        // POST: Areas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // лише авторизовані можуть редагувати
        public async Task<IActionResult> Edit(int id, [Bind("AreaName,Id")] Area area)
        {
            if (id != area.Id) return NotFound();

            // Перевірка на дублікати
            if (_context.Areas.Any(a => a.AreaName == area.AreaName && a.Id != area.Id))
            {
                ModelState.AddModelError("AreaName", "Дослідна область з такою назвою вже існує.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(area);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreaExists(area.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(area);
        }

        // GET: Areas/Delete/5
        [Authorize] // лише авторизовані можуть видаляти
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var area = await _context.Areas.FirstOrDefaultAsync(m => m.Id == id);
            if (area == null) return NotFound();

            return View(area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize] // лише авторизовані можуть видаляти
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null) return NotFound();

            try
            {
                _context.Areas.Remove(area);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["DeleteError"] = "Неможливо видалити область, оскільки існують пов'язані записи.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AreaExists(int id)
        {
            return _context.Areas.Any(e => e.Id == id);
        }
    }
}
