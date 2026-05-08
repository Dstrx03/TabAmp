using System;

namespace TabAmp.Shared.Validation.Extensions;

public static class ExceptionScopeExtensions
{
    public static Scope CaptureBy(this Exception error, ref Scope scope)
    {
        scope = scope.With(error);
        return scope;
    }

    public static bool ShouldStop(this Exception error, ref Scope scope) =>
        error.CaptureBy(ref scope).ShouldStop;
}
