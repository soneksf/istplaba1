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
    public class ResearchWorkImportService : IImportService<ResearchWork>
    {
        private readonly DblibraryContext _context;

        public ResearchWorkImportService(DblibraryContext context)
        {
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("Потік не може бути прочитаний.", nameof(stream));
            }

            using (var workbook = new XLWorkbook(stream))
            {
                // Припустимо, що дані знаходяться на першому листі
                var worksheet = workbook.Worksheet(1);
                // Припускаємо, що перший рядок – заголовки
                foreach (var row in worksheet.RowsUsed().Skip(1))
                {
                    // Зчитуємо дані з рядка (припустимо, що:
                    // 1-я клітинка – Title,
                    // 2-я – AreaName,
                    // 3-я – EmployeeFullName)
                    var title = row.Cell(1).GetString();
                    if (string.IsNullOrWhiteSpace(title))
                        continue; // пропускаємо порожні рядки

                    // Перевірка на дублікати (якщо потрібно)
                    if (_context.ResearchWorks.Any(r => r.Title == title))
                        continue;

                    ResearchWork researchWork = new ResearchWork
                    {
                        Title = title,
                        // Для Area та Employee потрібно знайти відповідні сутності за назвою,
                        // або створити нові, якщо їх немає.
                        // Приклад:
                        AreaId = await GetAreaIdAsync(row.Cell(2).GetString(), cancellationToken),
                        EmployeeId = await GetEmployeeIdAsync(row.Cell(3).GetString(), cancellationToken)
                    };

                    _context.ResearchWorks.Add(researchWork);
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<int> GetAreaIdAsync(string areaName, CancellationToken cancellationToken)
        {
            var area = await _context.Areas.FirstOrDefaultAsync(a => a.AreaName == areaName, cancellationToken);
            if (area == null)
            {
                area = new Area { AreaName = areaName };
                _context.Areas.Add(area);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return area.Id;
        }

        private async Task<int> GetEmployeeIdAsync(string employeeFullName, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.FullName == employeeFullName, cancellationToken);
            if (employee == null)
            {
                // Встановлюємо значення за замовчуванням для обов'язкових полів
                employee = new Employee
                {
                    FullName = employeeFullName,
                    Faculty = "Unknown",  // За замовчуванням, якщо немає даних
                    StartDate = DateOnly.FromDateTime(DateTime.Today), // Можна встановити поточну дату
                    DepartmentId = 1  // Наприклад, припустимо, існує відділ із Id = 1; або обробіть це окремо
                                      // LabId можна залишити null, якщо це допускається
                };
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return employee.Id;
        }

    }
}
