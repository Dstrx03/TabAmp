using System.Threading;
using System.Threading.Tasks;

namespace TabAmp.Engine.Core.FileSerialization;

public interface IFileSerializationService
{
    Task<T> ReadFileAsync<T>(string filePath, CancellationToken cancellationToken = default);
}
