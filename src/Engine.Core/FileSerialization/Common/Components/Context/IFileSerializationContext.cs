using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

internal interface IFileSerializationContext
{
    string FilePath { get; }
    CancellationToken CancellationToken { get; }
}
