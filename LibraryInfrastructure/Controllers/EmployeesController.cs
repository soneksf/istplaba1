using Microsoft.AspNetCore.Authorization; // Додаємо для використання [Authorize]
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryDomain.Models;
using LibraryInfrastructure;

namespace LibraryInfrastructure.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly DblibraryContext _context;

        public EmployeesController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Employees
        [AllowAnonymous] // або без атрибуту, якщо за замовчуванням усім дозволено
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Lab)
                .ToListAsync();
            return View(employees);
        }

        // GET: Employees/Details/5
        [AllowAnonymous] // дозволяє переглядати деталі неавторизованим
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Lab)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "admin")] // лише авторизовані можуть створювати
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName");
            ViewData["LabId"] = new SelectList(_context.Laboratories, "Id", "LabNumber");
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")] // лише авторизовані можуть створювати
        public async Task<IActionResult> Create([Bind("FullName,Faculty,StartDate,EndDate,DepartmentId,LabId,Id")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", employee.DepartmentId);
            ViewData["LabId"] = new SelectList(_context.Laboratories, "Id", "LabNumber", employee.LabId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "admin")] // лише авторизовані можуть редагувати
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", employee.DepartmentId);
            ViewData["LabId"] = new SelectList(_context.Laboratories, "Id", "LabNumber", employee.LabId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")] // лише авторизовані можуть редагувати
        public async Task<IActionResult> Edit(int id, [Bind("FullName,Faculty,StartDate,EndDate,DepartmentId,LabId,Id")] Employee employee)
        {
            if (id != employee.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", employee.DepartmentId);
            ViewData["LabId"] = new SelectList(_context.Laboratories, "Id", "LabNumber", employee.LabId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "admin")] // лише авторизовані можуть видаляти
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Lab)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")] // лише авторизовані можуть видаляти
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["DeleteError"] = "Не вдається видалити працівника, бо є пов’язані записи.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
