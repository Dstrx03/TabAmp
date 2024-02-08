using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

internal partial class FileSerializationContextBuilder
{
    private class FileSerializationContext : IFileSerializationContext
    {
        public string FilePath { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}
