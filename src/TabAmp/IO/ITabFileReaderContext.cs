using TabAmp.Models;

namespace TabAmp.IO;

public interface ITabFileReaderContext
{
    public PathInfo PathInfo { get; }
    public CancellationToken CancellationToken { get; }
}
