using ClosedXML.Excel;
using LibraryDomain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryInfrastructure.Services
{
    public class ResearchWorkExportService : IExportService<ResearchWork>
    {
        private readonly DblibraryContext _context;
        private static readonly string[] HeaderNames = { "Title", "Area", "Employee" };

        public ResearchWorkExportService(DblibraryContext context)
        {
            _context = context;
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Потік не доступний для запису.", nameof(stream));
            }

            // Завантажуємо всі дослідницькі роботи з відповідними Area та Employee
            var researchWorks = await _context.ResearchWorks
                .Include(r => r.Area)
                .Include(r => r.Employee)
                .ToListAsync(cancellationToken);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ResearchWorks");
                // Запис заголовків
                for (int i = 0; i < HeaderNames.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = HeaderNames[i];
                }
                worksheet.Row(1).Style.Font.Bold = true;

                int rowIndex = 2;
                foreach (var work in researchWorks)
                {
                    worksheet.Cell(rowIndex, 1).Value = work.Title;
                    worksheet.Cell(rowIndex, 2).Value = work.Area?.AreaName;
                    worksheet.Cell(rowIndex, 3).Value = work.Employee?.FullName;
                    rowIndex++;
                }
                workbook.SaveAs(stream);
            }
        }
    }
}
