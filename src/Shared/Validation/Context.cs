using System;
using System.Collections.Generic;

namespace TabAmp.Shared.Validation;

public readonly ref struct Context
{
    private readonly List<Exception>? _errors;

    public bool StopOnFirstError { get; }

    private Context(List<Exception>? errors, bool stopOnFirstError)
    {
        _errors = errors;
        StopOnFirstError = stopOnFirstError;
    }

    private Context WithError(Exception error)
    {
        var errors = _errors ?? [];
        errors.Add(error);

        return new(errors, stopOnFirstError: StopOnFirstError);
    }

    internal static Context Init_TODONAME(bool stopOnFirstError) => new(errors: null, stopOnFirstError: stopOnFirstError);
}
