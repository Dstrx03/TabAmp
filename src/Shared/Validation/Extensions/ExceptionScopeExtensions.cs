using System;

namespace TabAmp.Shared.Validation.Extensions;

public static class ExceptionScopeExtensions
{
    public static bool WithError_TODONAME(this Exception error, ref Scope scope)
    {
        scope = scope.WithError(error);
        return scope.StopOnFirstError;
    }
}
