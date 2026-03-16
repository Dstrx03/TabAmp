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

    internal Context WithError(Exception error)
    {
        if (StopOnFirstError)
            return this;

        var errors = _errors ?? [];
        errors.Add(error);

        return new(errors, stopOnFirstError: StopOnFirstError);
    }

    public Scope ToScope() => Scope.FromContext(this);
    internal ValidationResult ToResult() => new(_errors);

    internal static Context Init_TODONAME(bool stopOnFirstError) =>
        new(errors: null, stopOnFirstError: stopOnFirstError);
}
