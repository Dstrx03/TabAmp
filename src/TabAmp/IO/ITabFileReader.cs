using TabAmp.Commands;

namespace TabAmp.IO;

public interface ITabFileReader
{
    public Task<ReadTabFileResult> ReadAsync(string path, CancellationToken cancellationToken);
}
