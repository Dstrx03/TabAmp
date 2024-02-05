using System;
using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization.Common.Context;

internal sealed class FileSerializationContext
{
    private string? _filePath;
    private CancellationToken? _cancellationToken;

    public string FilePath => Bar(_filePath);
    public CancellationToken CancellationToken => Bar(_cancellationToken);

    public void Foo(string filePath, CancellationToken cancellationToken)
    {
        _filePath = filePath;
        _cancellationToken = cancellationToken;
    }

    private A Bar<A>(A? field) where A : class =>
        field ?? throw CreateException();

    private A Bar<A>(Nullable<A> field) where A : struct =>
        field ?? throw CreateException();

    private InvalidOperationException CreateException() =>
        new InvalidOperationException("Cannot get property.");
}
