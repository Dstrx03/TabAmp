using System;

namespace TabAmp.Shared.Validation;

internal readonly ref struct Errors
{
    private readonly object? _errors;
    private readonly int _length;

    private Errors(object? errors, int length) => (_errors, _length) = (errors, length);

    private bool IsEmpty => _length == 0;
    private bool IsSingle => _length == 1;
    private bool IsMany => _length > 1;

    private Errors Add(Exception error)
    {
        ArgumentNullException.ThrowIfNull(error);
        object? errors = null;
        var length = _length + 1;
        return new(errors, length);
    }

    private static Errors Empty => new();
}
