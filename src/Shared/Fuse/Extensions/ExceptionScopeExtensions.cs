using System;

namespace TabAmp.Shared.Fuse.Extensions;

public static class ExceptionScopeExtensions
{
    public static FuseScope CaptureBy(this Exception error, ref FuseScope scope)
    {
        scope = scope.With(error);
        return scope;
    }

    public static bool ShouldStop(this Exception error, ref FuseScope scope) =>
        error.CaptureBy(ref scope).ShouldStop;
}
