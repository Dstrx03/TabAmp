using System;
using TabAmp.Shared.Validation;
using TabAmp.Shared.Validation.Extensions;

namespace TabAmp.Cli.Console;

internal static class Tests
{
    public static void Run()
    {
        var result = SomeValidationMethod();
    }

    private static ValidationResult SomeValidationMethod(Scope scope = default)
    {
        new InvalidOperationException("some error").CaptureBy(ref scope);

        var result = InnerValidationMethod(scope.ToInner());
        result.CaptureBy(ref scope);

        return scope.ToResult();
    }

    private static ValidationResult InnerValidationMethod(Scope scope = default)
    {
        return scope.ToResult();
    }
}
