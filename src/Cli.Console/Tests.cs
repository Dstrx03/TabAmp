using System;
using TabAmp.Shared.Validation;
using TabAmp.Shared.Validation.Extensions;

namespace TabAmp.Cli.Console;

internal static class Tests
{
    public static ValidationResult SomeValidationMethod(bool stopOnFirstError = false)
    {
        var scope = Scope.Init_TODONAME(stopOnFirstError);

        if (SomeValidationMethodA(scope).WithError_TODONAME(ref scope))
            return scope;

        if (true)
        {
            var error = new InvalidOperationException("Some error...");
            if (error.WithError_TODONAME(ref scope))
                return scope;
        }

        return scope;
    }

    private static ScopeResult SomeValidationMethodA(Context context)
    {
        var scope = context.ToScope();

        if (SomeValidationMethodB(scope).WithError_TODONAME(ref scope))
            return scope;

        if (true)
        {
            var error = new InvalidOperationException("A error...");
            if (error.WithError_TODONAME(ref scope))
                return scope;
        }

        return scope;
    }

    private static ScopeResult SomeValidationMethodB(Context context)
    {
        var scope = context.ToScope();

        if (true)
        {
            var error = new InvalidOperationException("B error...");
            if (error.WithError_TODONAME(ref scope))
                return scope;
        }

        return scope;
    }
}
