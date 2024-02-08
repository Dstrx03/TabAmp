using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

internal sealed partial class FileSerializationContextBuilder
{
    private readonly FileSerializationContext _context;

    public FileSerializationContextBuilder(IServiceProvider serviceProvider) =>
        _context = serviceProvider.GetRequiredService<FileSerializationContext>();

    public bool IsContextBuilt { get; private set; }

    public IFileSerializationContext GetContext()
    {
        if (!IsContextBuilt)
            throw new InvalidOperationException($"Cannot provide not built '{typeof(FileSerializationContext)}'.");
        return _context;
    }

    public void BuildContext(string filePath, CancellationToken cancellationToken)
    {
        if (IsContextBuilt)
            throw new InvalidOperationException($"Cannot build '{typeof(FileSerializationContext)}', it's already built for its scope.");
        SetContextData(filePath, cancellationToken);
        IsContextBuilt = true;
    }

    private void SetContextData(string filePath, CancellationToken cancellationToken)
    {
        _context.FilePath = filePath;
        _context.CancellationToken = cancellationToken;
    }
}
