using System.Threading;
using TabAmp.Engine.Core.Score;

namespace TabAmp.Engine.Core.FileSerialization;

internal sealed class FileSerializationContext
{
    public string FilePath { get; set; }
    public Gp5Score FileData { get; set; }
    public CancellationToken CancellationToken { get; set; }
}
