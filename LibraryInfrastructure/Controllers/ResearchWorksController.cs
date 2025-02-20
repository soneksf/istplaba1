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
    public class ResearchWorksController : Controller
    {
        private readonly DblibraryContext _context;

        public ResearchWorksController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: ResearchWorks
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.ResearchWorks.Include(r => r.Area).Include(r => r.Employee)/*.Include(r => r.Publisher)*/;
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: ResearchWorks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load the researchWork and its Employee (to access DepartmentId).
            var researchWork = await _context.ResearchWorks
                .Include(r => r.Employee)      // So we can access Employee.DepartmentId
                .FirstOrDefaultAsync(r => r.Id == id);

            if (researchWork == null)
            {
                return NotFound();
            }

            // Retrieve the departmentId from the researchWork's Employee.
            // If Employee is null or DepartmentId is unknown, handle that scenario.
            int departmentId = researchWork.Employee.DepartmentId;

            // Now redirect to the DepartmentsController Details action with that ID.
            return RedirectToAction("Details", "Departments", new { id = departmentId });
        }



        // GET: ResearchWorks/Create
        public IActionResult Create()
        {
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "AreaName");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "FullName");
            return View();
        }

        // POST: ResearchWorks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,EmployeeId,PublisherId,AreaId,Id")] ResearchWork researchWork)
        {
            if (ModelState.IsValid)
            {
                _context.Add(researchWork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "AreaName", researchWork.AreaId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", researchWork.EmployeeId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "FullName"/*, researchWork.PublisherId*/);
            return View(researchWork);
        }

        // GET: ResearchWorks/Edit/5
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
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "FullName"/*, researchWork.PublisherId*/);
            return View(researchWork);
        }

        // POST: ResearchWorks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,EmployeeId,PublisherId,AreaId,Id")] ResearchWork researchWork)
        {
            if (id != researchWork.Id)
            {
                return NotFound();
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
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "FullName"/*, researchWork.PublisherId*/);
            return View(researchWork);
        }

        // GET: ResearchWorks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var researchWork = await _context.ResearchWorks
                .Include(r => r.Area)
                .Include(r => r.Employee)
             //   .Include(r => r.Publisher)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var researchWork = await _context.ResearchWorks.FindAsync(id);
            if (researchWork != null)
            {
                _context.ResearchWorks.Remove(researchWork);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResearchWorkExists(int id)
        {
            return _context.ResearchWorks.Any(e => e.Id == id);
        }
    }
}
