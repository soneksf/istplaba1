/*using System.Text;
using Xceed.Words.NET;
using Microsoft.EntityFrameworkCore;
using LibraryDomain.Models;

namespace LibraryInfrastructure.Services
{
    public class ResearchWorkDocImportService : IImportService<ResearchWork>
    {
        private readonly DblibraryContext _context;

        public ResearchWorkDocImportService(DblibraryContext context)
        {
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            // 1) Validate the stream
            if (!stream.CanRead)
            {
                throw new ArgumentException("Stream is not readable.", nameof(stream));
            }

            // 2) Load the DOCX file from the stream
            using (var doc = DocX.Load(stream))
            {
                // 3) Parse the doc to extract data
                //    Typically, you'll look for tables, paragraphs, or both.

                // For example, let's assume there's exactly one table with columns:
                // [0] Title
                // [1] EmployeeFullName
                // [2] AreaName

                if (doc.Tables.Count == 0)
                {
                    throw new InvalidOperationException("No tables found in the .docx document.");
                }

                var table = doc.Tables[0]; // Take the first table
                // Optionally, if there's a header row, skip row 0.

                for (int rowIndex = 1; rowIndex < table.RowCount; rowIndex++)
                {
                    var row = table.Rows[rowIndex];
                    // Read cell text (the first paragraph in each cell)
                    var title = row.Cells[0].Paragraphs[0].Text.Trim();
                    var employeeName = row.Cells[1].Paragraphs[0].Text.Trim();
                    var areaName = row.Cells[2].Paragraphs[0].Text.Trim();

                    // 4) Upsert data in DB
                    //    a) find or create Employee
                    var employee = await _context.Employees
                        .FirstOrDefaultAsync(e => e.FullName == employeeName, cancellationToken);

                    if (employee == null)
                    {
                        employee = new Employee
                        {
                            FullName = employeeName,
                            Faculty = "ImportedFromDoc"
                        };
                        _context.Employees.Add(employee);
                    }

                    //    b) find or create Area
                    var area = await _context.Areas
                        .FirstOrDefaultAsync(a => a.AreaName == areaName, cancellationToken);

                    if (area == null)
                    {
                        area = new Area { AreaName = areaName };
                        _context.Areas.Add(area);
                    }

                    //    c) create new ResearchWork
                    var researchWork = new ResearchWork
                    {
                        Title = title,
                        Employee = employee,
                        Area = area
                    };
                    _context.ResearchWorks.Add(researchWork);
                }
            }

            // 5) Save changes
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
*/