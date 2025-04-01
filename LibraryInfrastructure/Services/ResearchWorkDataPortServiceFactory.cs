using LibraryDomain.Models;

namespace LibraryInfrastructure.Services
{
    public class ResearchWorkDataPortServiceFactory : IDataPortServiceFactory<ResearchWork>
    {
        private readonly DblibraryContext _context;

        public ResearchWorkDataPortServiceFactory(DblibraryContext context)
        {
            _context = context;
        }

        public IImportService<ResearchWork> GetImportService(string contentType)
        {
            if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new ResearchWorkImportService(_context);
            }
            throw new NotImplementedException($"Імпорт для типу {contentType} не реалізовано.");
        }

        public IExportService<ResearchWork> GetExportService(string contentType)
        {
            if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new ResearchWorkExportService(_context);
            }
            throw new NotImplementedException($"Експорт для типу {contentType} не реалізовано.");
        }
    }
}
