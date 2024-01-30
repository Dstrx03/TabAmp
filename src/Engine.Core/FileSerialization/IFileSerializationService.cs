using System.Threading;
using System.Threading.Tasks;
using TabAmp.Engine.Core.Score;

namespace TabAmp.Engine.Core.FileSerialization;

public interface IFileSerializationService
{
    Task<Gp5Score> ReadFileAsync(string path, CancellationToken cancellationToken = default);
    Task WriteFileAsync(Gp5Score input, string path, CancellationToken cancellationToken = default);
}
