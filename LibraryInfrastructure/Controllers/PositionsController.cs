using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryDomain.Models;
using LibraryInfrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryInfrastructure.Controllers
{
    public class PositionsController : Controller
    {
        private readonly DblibraryContext _context;

        public PositionsController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Positions
        // Доступний усім
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.Positions.Include(p => p.Employee);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: Positions/Details/5
        // Доступний усім
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // GET: Positions/Create
        // Лише для авторизованих користувачів
        [Authorize]
        public IActionResult Create(int? employeeId)
        {
            if (employeeId.HasValue)
            {
                ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", employeeId.Value);
            }
            else
            {
                ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            }

            return View();
        }

        // POST: Positions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,PositionName,StartDate,EndDate,EmployeeId")] Position position)
        {
            // Перевірка: для одного працівника може бути лише одна посада
            bool positionExists = _context.Positions.Any(p => p.EmployeeId == position.EmployeeId);
            if (positionExists)
            {
                ModelState.AddModelError("EmployeeId", "This employee already has a position.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(position);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", position.EmployeeId);
            return View(position);
        }

        // GET: Positions/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", position.EmployeeId);
            return View(position);
        }

        // POST: Positions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PositionName,StartDate,EndDate,EmployeeId")] Position position)
        {
            if (id != position.Id)
            {
                return NotFound();
            }

            // Перевірка: інша посада для цього працівника не повинна існувати
            bool positionExists = _context.Positions.Any(p => p.EmployeeId == position.EmployeeId && p.Id != position.Id);
            if (positionExists)
            {
                ModelState.AddModelError("EmployeeId", "This employee already has a position.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(position);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PositionExists(position.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", position.EmployeeId);
            return View(position);
        }

        // GET: Positions/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position != null)
            {
                _context.Positions.Remove(position);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["DeleteError"] = "Неможливо видалити дослідницьку роботу, оскільки існують пов'язані записи.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PositionExists(int id)
        {
            return _context.Positions.Any(e => e.Id == id);
        }
    }
}
