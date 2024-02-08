using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

internal partial class FileSerializationContextBuilder
{
    private class ScopedFileSerializationContext : FileSerializationContext
    {
        public bool IsContextBuilt { get; private set; }

        public void SetContextData(string filePath, CancellationToken cancellationToken)
        {
            FilePath = filePath;
            CancellationToken = cancellationToken;
        }

        public void SetContextBuilt() => IsContextBuilt = true;
    }
}
