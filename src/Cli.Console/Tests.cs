using System;
using TabAmp.Shared.Validation;
using TabAmp.Shared.Validation.Extensions;

namespace TabAmp.Cli.Console;

internal static class Tests
{
    public static ValidationResult SomeValidationMethod(bool stopOnFirstError = false)
    {
        var scope = Scope.Init_TODONAME(stopOnFirstError);

        if (true)
        {
            var error = new InvalidOperationException("A error...");
            if (error.WithError_TODONAME(ref scope))
                return scope;
        }

        return scope;
    }
}
