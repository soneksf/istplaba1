using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        // GET: Employees
        public async Task<IActionResult> Index(int? departmentId, string? departmentName)
        {
            // Start with a base query
            var employeesQuery = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Lab)
                .AsQueryable();

            // If departmentId is given, filter
            if (departmentId != null)
            {
                employeesQuery = employeesQuery.Where(e => e.DepartmentId == departmentId);
                // Store these in ViewBag if you want to show them in the view
                ViewBag.DepartmentId = departmentId;
                ViewBag.DepartmentName = departmentName;
            }

            // Execute the query
            var employees = await employeesQuery.ToListAsync();
            return View(employees);
        }


        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Lab)
                .Include(e => e.Positions)  // <-- Include Positions here
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }


        // GET: Employees/Create
        public IActionResult Create(int? departmentId)
        {
            // Department dropdown (if you need it)
            if (departmentId.HasValue)
            {
                ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "DepartmentName", departmentId.Value);
            }
            else
            {
                ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "DepartmentName");
            }

            // Laboratory dropdown
            ViewBag.LabId = new SelectList(_context.Laboratories, "Id", "LabNumber");

            return View();
        }




        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", employee.DepartmentId);
            ViewData["LabId"] = new SelectList(_context.Laboratories, "Id", "LabNumber", employee.LabId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FullName,Faculty,StartDate,EndDate,DepartmentId,LabId,Id")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", employee.DepartmentId);
            ViewData["LabId"] = new SelectList(_context.Laboratories, "Id", "LabNumber", employee.LabId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Lab)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
