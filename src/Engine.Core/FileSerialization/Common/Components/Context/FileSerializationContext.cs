using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

internal abstract class FileSerializationContext
{
    private protected FileSerializationContext(string filePath, CancellationToken cancellationToken)
    {
        FilePath = filePath;
        CancellationToken = cancellationToken;
    }

    public string FilePath { get; }
    public CancellationToken CancellationToken { get; }
}
