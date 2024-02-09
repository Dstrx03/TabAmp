using System;
using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

internal sealed partial class FileSerializationContextBuilder
{
    private ScopedFileSerializationContext _context;

    public FileSerializationContextBuilder(IServiceProvider serviceProvider)
    {
    }

    public FileSerializationContext Context => _context is null
        ? throw new InvalidOperationException($"Cannot provide not built '{typeof(ScopedFileSerializationContext)}'.")
        : _context;

    public void BuildContext(string filePath, CancellationToken cancellationToken)
    {
        if (_context is not null)
            throw new InvalidOperationException($"Cannot build '{typeof(ScopedFileSerializationContext)}', it's already built for its scope.");

        _context = new ScopedFileSerializationContext
        {
            FilePath = filePath,
            CancellationToken = cancellationToken
        };
    }


}
