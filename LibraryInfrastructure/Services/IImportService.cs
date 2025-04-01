using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryInfrastructure.Services
{
    public interface IImportService<TEntity>
        where TEntity : class
    {
        Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
    }
}
