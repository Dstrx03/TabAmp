using System;

namespace TabAmp.Shared.Fuse;

internal readonly ref struct FuseContext
{
    internal FuseErrors Errors { get; }
    internal bool StopOnFirstError { get; }

    internal FuseContext(bool stopOnFirstError) => StopOnFirstError = stopOnFirstError;

    private FuseContext(FuseErrors errors, bool stopOnFirstError)
    {
        Errors = errors;
        StopOnFirstError = stopOnFirstError;
    }

    internal FuseContext With(Exception error) => new(Errors.Add(error), stopOnFirstError: StopOnFirstError);

    internal FuseContext ToInner() => new(Errors.ToInner(), stopOnFirstError: StopOnFirstError);

    internal FuseContext FromOuter(FuseContext outer)
    {
        if (outer.StopOnFirstError != StopOnFirstError)
            throw OuterAndCurrentScopesNotCompatibleException();

        return new(Errors.FromOuter(outer.Errors), stopOnFirstError: StopOnFirstError);
    }

    private static InvalidOperationException OuterAndCurrentScopesNotCompatibleException() =>
        new("The outer and current scopes are not compatible.");
}
