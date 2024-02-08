using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

internal sealed partial class FileSerializationContextBuilder
{
    private readonly ScopedFileSerializationContext _context;

    public FileSerializationContextBuilder(IServiceProvider serviceProvider) =>
        _context = serviceProvider.GetRequiredService<ScopedFileSerializationContext>();

    public FileSerializationContext Context => !IsContextBuilt
        ? throw new InvalidOperationException($"Cannot provide not built '{typeof(ScopedFileSerializationContext)}'.")
        : _context;

    public bool IsContextBuilt => _context.IsContextBuilt;

    public void BuildContext(string filePath, CancellationToken cancellationToken)
    {
        if (IsContextBuilt)
            throw new InvalidOperationException($"Cannot build '{typeof(ScopedFileSerializationContext)}', it's already built for its scope.");

        _context.SetContextData(filePath, cancellationToken);
        _context.SetContextBuilt();
    }


}
