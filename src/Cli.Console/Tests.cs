using System;
using System.Collections.Generic;
using TabAmp.Shared.Validation;
using TabAmp.Shared.Validation.Extensions;

namespace TabAmp.Cli.Console;

internal static class Tests
{
    private static readonly HashSet<string> _errors = [
        "SomeValidationMethod_A",
        "SomeInnerValidationMethod_A",
        "SomeInnerInnerValidationMethod_A",
        "SomeInnerInnerValidationMethod_B"
    ];

    public static void Run()
    {
        var result = SomeValidationMethod();
    }

    private static ValidationResult SomeValidationMethod(Scope scope = default)
    {
        if (HasError("SomeValidationMethod_A"))
        {
            var error = new InvalidOperationException("SomeValidationMethod error A.");
            if (error.CaptureBy(ref scope).ShouldStop)
                return scope.ToResult();
        }

        if (SomeInnerValidationMethod(scope.ToInner()).CaptureBy(ref scope).ShouldStop)
            return scope.ToResult();

        return scope.ToResult();
    }

    private static ValidationResult SomeInnerValidationMethod(Scope scope = default)
    {
        if (HasError("SomeInnerValidationMethod_A"))
        {
            var error = new InvalidOperationException("SomeInnerValidationMethod error A.");
            if (error.CaptureBy(ref scope).ShouldStop)
                return scope.ToResult();
        }

        if (SomeInnerInnerValidationMethod(scope.ToInner()).CaptureBy(ref scope).ShouldStop)
            return scope.ToResult();

        return scope.ToResult();
    }

    private static ValidationResult SomeInnerInnerValidationMethod(Scope scope = default)
    {
        if (HasError("SomeInnerInnerValidationMethod_A"))
        {
            var error = new InvalidOperationException("SomeInnerInnerValidationMethod error A.");
            if (error.CaptureBy(ref scope).ShouldStop)
                return scope.ToResult();
        }

        if (HasError("SomeInnerInnerValidationMethod_B"))
        {
            var error = new InvalidOperationException("SomeInnerInnerValidationMethod error B.");
            if (error.CaptureBy(ref scope).ShouldStop)
                return scope.ToResult();
        }

        return scope.ToResult();
    }

    private static bool HasError(string error) => _errors.Contains(error);
}
