using System;
using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

internal class ScopedFileSerializationContextContainer
{
    private ScopedFileSerializationContext? _context;

    public FileSerializationContext Context
    {
        get
        {
            if (!IsConstructed)
                throw new InvalidOperationException($"{nameof(FileSerializationContext)} is not constructed.");

            return _context!;
        }
    }

    public bool IsConstructed => _context is not null;

    public void CreateContext(string filePath, CancellationToken cancellationToken)
    {
        if (IsConstructed)
            throw new InvalidOperationException($"{nameof(FileSerializationContext)} is already constructed.");

        _context = new(filePath, cancellationToken);
    }

    private class ScopedFileSerializationContext(string filePath, CancellationToken cancellationToken)
        : FileSerializationContext(filePath, cancellationToken)
    {
    }
}
