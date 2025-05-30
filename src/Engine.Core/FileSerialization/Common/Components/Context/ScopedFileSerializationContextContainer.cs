﻿using System;
using System.Diagnostics;
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
                throw ContextDoesNotExistException;

            return _context!;
        }
    }

    public bool HasContext => _context is not null;

    public void CreateContext(string filePath, CancellationToken cancellationToken)
    {
        if (HasContext)
            throw ContextAlreadyExistsException;

        _context = new(filePath, cancellationToken);
    }

    private class ScopedFileSerializationContext(string filePath, CancellationToken cancellationToken)
        : FileSerializationContext(filePath, cancellationToken)
    {
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private InvalidOperationException ContextDoesNotExistException =>
        new($"Cannot access the context: {nameof(FileSerializationContext)} does not exist in the current scope " +
            $"and must be initialized via {nameof(CreateContext)}.");

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private InvalidOperationException ContextAlreadyExistsException =>
        new($"Cannot create the context: {nameof(FileSerializationContext)} already exists in the current scope " +
            $"with {nameof(_context.FilePath)}: '{_context!.FilePath}' and cannot be initialized again.");
}
