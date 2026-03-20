using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TabAmp.Shared.Validation;

internal readonly ref struct Errors
{
    private readonly object? _errors;
    private readonly int _length;

    private Errors(object? errors, int length) =>
        (_errors, _length) = (errors, length);

    private bool IsEmpty => _length == 0;
    private bool IsSingle => _length == 1;
    private bool IsMany => _length > 1;

    private Errors Add(Exception error)
    {
        ArgumentNullException.ThrowIfNull(error);

        object errors = this switch
        {
            _ when IsEmpty => error,
            _ when IsSingle => AddAsSingle(error),
            _ when IsMany => AddAsMany(error),
            _ => throw new UnreachableException()
        };

        return new(errors, _length + 1);
    }

    private List<Exception> AddAsSingle(Exception error) =>
        [(_errors as Exception)!, error];

    private List<Exception> AddAsMany(Exception error)
    {
        var errors = (_errors as List<Exception>)!;
        errors.Add(error);

        return errors;
    }

    private static Errors Empty => new();
}
