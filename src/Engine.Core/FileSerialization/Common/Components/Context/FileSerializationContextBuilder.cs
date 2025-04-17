using System;
using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

internal class FileSerializationContextBuilder
{
    private ScopedFileSerializationContext _context;

    public bool IsConstructed => _context is not null;

    public FileSerializationContext GetConstructedContext()
    {
        if (!IsConstructed)
            throw new InvalidOperationException($"{nameof(FileSerializationContext)} is not constructed.");

        return _context;
    }

    public void ConstructContext(string filePath, CancellationToken cancellationToken)
    {
        if (IsConstructed)
            throw new InvalidOperationException($"{nameof(FileSerializationContext)} is already constructed.");

        _context = new(filePath, cancellationToken);
    }

    private class ScopedFileSerializationContext : FileSerializationContext
    {
        public ScopedFileSerializationContext(string filePath, CancellationToken cancellationToken)
        {
            FilePath = filePath;
            CancellationToken = cancellationToken;
        }
    }
}
