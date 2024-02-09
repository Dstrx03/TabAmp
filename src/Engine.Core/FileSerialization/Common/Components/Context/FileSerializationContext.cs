using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

internal abstract class FileSerializationContext
{
    internal FileSerializationContext() { }

    public string FilePath { get; init; }
    public CancellationToken CancellationToken { get; init; }
}
