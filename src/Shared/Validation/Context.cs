using System;

namespace TabAmp.Shared.Validation;

internal readonly ref struct Context
{
    internal Errors Errors { get; }
    internal bool StopOnFirstError { get; }

    internal Context(bool stopOnFirstError) => StopOnFirstError = stopOnFirstError;

    private Context(Errors errors, bool stopOnFirstError)
    {
        Errors = errors;
        StopOnFirstError = stopOnFirstError;
    }

    internal Context With(Exception error) => new(Errors.Add(error), stopOnFirstError: StopOnFirstError);

    internal Context ToInner() => new(Errors.ToInner(), stopOnFirstError: StopOnFirstError);
}
