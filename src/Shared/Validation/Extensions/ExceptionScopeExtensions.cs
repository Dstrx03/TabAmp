using System;

namespace TabAmp.Shared.Validation.Extensions;

public static class ExceptionScopeExtensions
{
    public static Scope CaptureBy(this Exception error, ref Scope scope)
    {
        scope = scope.With(error);
        return scope;
    }
}
