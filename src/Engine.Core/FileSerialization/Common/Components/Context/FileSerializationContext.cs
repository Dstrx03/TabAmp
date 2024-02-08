using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

internal abstract class FileSerializationContext
{
    internal FileSerializationContext() { }

    public string FilePath { get; protected set; }
    public CancellationToken CancellationToken { get; protected set; }
}
