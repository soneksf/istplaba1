using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryInfrastructure;
using LibraryDomain.Models;

namespace LibraryInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DblibraryContext _context;

        public record CountByYearResponseItem(string Year, int Count);

        public record CountByEmployeeResponseItem(string EmployeeName, int Count);

        public ChartController(DblibraryContext context)
        {
            _context = context;
        }

        [HttpGet("employees-by-year")]
        public async Task<ActionResult<IEnumerable<CountByYearResponseItem>>> GetEmployeesByYear()
        {
            var data = await _context.Employees
                // If StartDate is non-nullable, no need for HasValue:
                .GroupBy(e => e.StartDate.Year)
                .Select(g => new CountByYearResponseItem(
                    g.Key.ToString(),
                    g.Count()
                ))
                .ToListAsync();

            return Ok(data);
        }


        [HttpGet("research-works-per-employee")]
        public async Task<ActionResult<IEnumerable<CountByEmployeeResponseItem>>> GetResearchWorksPerEmployee()
        {
            
            var data = await _context.ResearchWorks
                .GroupBy(r => r.EmployeeId)
                .Select(g => new
                {
                    EmployeeName = g.Select(x => x.Employee.FullName).FirstOrDefault() ?? "No Name",
                    Count = g.Count()
                })
                .ToListAsync();

            var result = data.Select(d => new CountByEmployeeResponseItem(d.EmployeeName, d.Count)).ToList();

            return Ok(result);
        }

        public record PieDataResponseItem(string Label, int Value);

        [HttpGet("employees-by-department")]
        public async Task<ActionResult<IEnumerable<PieDataResponseItem>>> GetEmployeesByDepartment()
        {
            var data = await _context.Employees
                .GroupBy(e => e.DepartmentId)
                .Select(g => new
                {
                    DepartmentName = g.Select(e => e.Department.DepartmentName).FirstOrDefault() ?? "Без катедри",
                    Count = g.Count()
                })
                .ToListAsync();

            var result = data.Select(d => new PieDataResponseItem(d.DepartmentName, d.Count)).ToList();
            return Ok(result);
        }

    }
}
