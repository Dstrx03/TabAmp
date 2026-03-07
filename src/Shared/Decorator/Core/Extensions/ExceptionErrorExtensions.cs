using System;
using System.Collections.Generic;

namespace TabAmp.Shared.Decorator.Core.Extensions;

internal static class ExceptionErrorExtensions
{
    internal static bool ShouldStopOn(this Exception? source, out Exception? error)
    {
        error = source;
        return error is not null;
    }

    internal static bool TryAddTo(this Exception error, ref List<Exception>? errors, bool stopOnFirstError)
    {
        if (stopOnFirstError)
            return false;

        errors ??= [];
        errors.Add(error);

        return true;
    }
}
