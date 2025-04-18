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
            if (!HasContext)
                throw new InvalidOperationException($"Cannot access the context: {nameof(FileSerializationContext)} does not exist in the current scope and must be initialized via {nameof(CreateContext)}.");

            return _context!;
        }
    }

    public bool HasContext => _context is not null;

    public void CreateContext(string filePath, CancellationToken cancellationToken)
    {
        if (HasContext)
            throw new InvalidOperationException($"Cannot create the context: {nameof(FileSerializationContext)} already exists in the current scope and cannot be initialized again.");

        _context = new(filePath, cancellationToken);
    }

    private class ScopedFileSerializationContext(string filePath, CancellationToken cancellationToken)
        : FileSerializationContext(filePath, cancellationToken)
    {
    }
}
