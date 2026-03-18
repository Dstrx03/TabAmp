using System;

namespace TabAmp.Shared.Validation;

internal readonly ref struct ErrorsCollection
{
    private readonly object? _errors;
    private readonly int _length;

    private ErrorsCollection(object? errors, int length) => (_errors, _length) = (errors, length);

    internal ErrorsCollection Add(Exception error)
    {
        object? errors = null;
        int length = 0;

        throw new NotImplementedException();
        return new(errors, length);
    }
}
