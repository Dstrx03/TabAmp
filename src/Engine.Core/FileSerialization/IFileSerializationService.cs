using System.Threading;
using System.Threading.Tasks;

namespace TabAmp.Engine.Core.FileSerialization;

public interface IFileSerializationService
{
    Task<TFileData> ReadFileAsync<TFileData>(string path, CancellationToken cancellationToken = default);

    //Task WriteFileAsync(Gp5Score input, string path, CancellationToken cancellationToken = default);
}
