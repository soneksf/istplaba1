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

        // Existing record for the employees-by-year chart
        public record CountByYearResponseItem(string Year, int Count);

        // New record for the research-works-per-employee chart
        public record CountByEmployeeResponseItem(string EmployeeName, int Count);

        public ChartController(DblibraryContext context)
        {
            _context = context;
        }

        // Existing method: employees-by-year
        [HttpGet("employees-by-year")]
        public async Task<ActionResult<IEnumerable<CountByYearResponseItem>>> GetEmployeesByYear()
        {
            var data = await _context.Employees
                .Where(e => e.StartDate.HasValue)
                .GroupBy(e => e.StartDate.Value.Year)
                .Select(g => new CountByYearResponseItem(
                    g.Key.ToString(),
                    g.Count()
                ))
                .ToListAsync();

            return Ok(data);
        }

        // ===========================================
        // NEW endpoint: research-works-per-employee
        // ===========================================
        [HttpGet("research-works-per-employee")]
        public async Task<ActionResult<IEnumerable<CountByEmployeeResponseItem>>> GetResearchWorksPerEmployee()
        {
            // Group research works by EmployeeId
            // Then get the Employee's FullName from the grouped items
            // and count how many research works are in each group.
            var data = await _context.ResearchWorks
                .GroupBy(r => r.EmployeeId)
                .Select(g => new
                {
                    EmployeeName = g.Select(x => x.Employee.FullName).FirstOrDefault() ?? "No Name",
                    Count = g.Count()
                })
                .ToListAsync();

            // Transform anonymous objects to the new record
            var result = data.Select(d => new CountByEmployeeResponseItem(d.EmployeeName, d.Count)).ToList();

            return Ok(result);
        }
    }
}
