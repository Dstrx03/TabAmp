using System;
using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

internal partial class FileSerializationContextBuilder
{
    private ScopedFileSerializationContext _context;

    public bool IsConstructed { get; private set; }

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

        CreateContext(filePath, cancellationToken);
        IsConstructed = true;
    }

    private void CreateContext(string filePath, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(filePath) || string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));

        _context = new ScopedFileSerializationContext
        {
            FilePath = filePath,
            CancellationToken = cancellationToken
        };
    }

    private class ScopedFileSerializationContext : FileSerializationContext
    {
    }
}
