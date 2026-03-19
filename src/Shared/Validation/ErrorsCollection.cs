using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TabAmp.Shared.Validation;

internal readonly ref struct ErrorsCollection
{
    private readonly object? _errors;
    private readonly int _length;

    private ErrorsCollection(object? errors, int length) => (_errors, _length) = (errors, length);

    internal ErrorsCollection Add(Exception error)
    {
        object? errors = null;
        int length = _length + 1;

        if (_length == -1)
        {
            errors = error;
        }
        else if (_length == 0)
        {
            errors = (List<Exception>)[(Exception)_errors!, error];
        }
        else if (_length > 0)
        {
            List<Exception> list = (List<Exception>)_errors;
            list.Add(error);
        }
        else
        {
            throw new UnreachableException();
        }

        return new(errors, length);
    }
}
