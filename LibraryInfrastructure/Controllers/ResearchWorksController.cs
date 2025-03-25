using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryDomain.Models;
using LibraryInfrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryInfrastructure.Controllers
{
    public class ResearchWorksController : Controller
    {

        private readonly DblibraryContext _context;

        public ResearchWorksController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: ResearchWorks
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.ResearchWorks
                .Include(r => r.Area)
                .Include(r => r.Employee);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: ResearchWorks/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var researchWork = await _context.ResearchWorks
                .Include(r => r.Employee)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (researchWork == null)
            {
                return NotFound();
            }

            int departmentId = researchWork.Employee.DepartmentId;
            // Якщо потрібно перенаправляти на деталі кафедри:
            return RedirectToAction("Details", "Departments", new { id = departmentId });
            // Або просто: return View(researchWork);
        }

        // GET: ResearchWorks/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "AreaName");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View();
        }

        // POST: ResearchWorks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Title,EmployeeId,AreaId,Id")] ResearchWork researchWork)
        {
            if (_context.ResearchWorks.Any(r => r.Title == researchWork.Title))
            {
                ModelState.AddModelError("Title", "Дослідницька робота з таким заголовком вже існує.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(researchWork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "AreaName", researchWork.AreaId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", researchWork.EmployeeId);
            return View(researchWork);
        }

        // GET: ResearchWorks/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var researchWork = await _context.ResearchWorks.FindAsync(id);
            if (researchWork == null)
            {
                return NotFound();
            }
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "AreaName", researchWork.AreaId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", researchWork.EmployeeId);
            return View(researchWork);
        }

        // POST: ResearchWorks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Title,EmployeeId,AreaId,Id")] ResearchWork researchWork)
        {
            if (id != researchWork.Id)
            {
                return NotFound();
            }

            if (_context.ResearchWorks.Any(r => r.Title == researchWork.Title && r.Id != researchWork.Id))
            {
                ModelState.AddModelError("Title", "Дослідницька робота з таким заголовком вже існує.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(researchWork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResearchWorkExists(researchWork.Id))
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
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "AreaName", researchWork.AreaId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", researchWork.EmployeeId);
            return View(researchWork);
        }

        // GET: ResearchWorks/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var researchWork = await _context.ResearchWorks
                .Include(r => r.Area)
                .Include(r => r.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (researchWork == null)
            {
                return NotFound();
            }

            return View(researchWork);
        }

        // POST: ResearchWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var researchWork = await _context.ResearchWorks.FindAsync(id);
            if (researchWork == null)
            {
                return NotFound();
            }
            try
            {
                _context.ResearchWorks.Remove(researchWork);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["DeleteError"] = "Неможливо видалити дослідницьку роботу, оскільки існують пов'язані записи.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ResearchWorkExists(int id)
        {
            return _context.ResearchWorks.Any(e => e.Id == id);
        }
    }
}
