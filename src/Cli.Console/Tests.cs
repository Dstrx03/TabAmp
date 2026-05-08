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
        "SomeInnerInnerValidationMethod_B",
        "SomeValueValidationMethod_A",
        "SomeValueValidationMethod_B",
        "SomeInnerValueValidationMethod_A"
    ];

    public static void Run()
    {
        var stopOnFirstError = false;

        var result = SomeValidationMethod(new(stopOnFirstError: stopOnFirstError));
        var valueResult = SomeValueValidationMethod(new(stopOnFirstError: stopOnFirstError));

        System.Console.WriteLine($"\nresult: {result.IsValid} (stopOnFirstError: {stopOnFirstError})");
        foreach (var error in result.Errors)
            System.Console.WriteLine($" - {error.Message}");

        System.Console.WriteLine($"\nvalueResult: {valueResult.IsValid}, '{valueResult.Value}' (stopOnFirstError: {stopOnFirstError})");
        foreach (var error in valueResult.Errors)
            System.Console.WriteLine($" - {error.Message}");
    }

    private static ValidationResult SomeValidationMethod(Scope scope = default)
    {
        if (HasError("SomeValidationMethod_A"))
        {
            var error = new InvalidOperationException("SomeValidationMethod error A.");
            if (error.ShouldStop(ref scope))
                return scope;
        }

        if (SomeInnerValidationMethod(scope.ToInner()).ShouldStop(ref scope))
            return scope;

        if (SomeValueValidationMethod(scope.ToInner()).ShouldStop(ref scope))
            return scope;

        return scope;
    }

    private static ValidationResult SomeInnerValidationMethod(Scope scope = default)
    {
        if (HasError("SomeInnerValidationMethod_A"))
        {
            var error = new InvalidOperationException("SomeInnerValidationMethod error A.");
            if (error.ShouldStop(ref scope))
                return scope;
        }

        if (SomeInnerInnerValidationMethod(scope.ToInner()).ShouldStop(ref scope))
            return scope;

        return scope;
    }

    private static ValidationResult SomeInnerInnerValidationMethod(Scope scope = default)
    {
        if (HasError("SomeInnerInnerValidationMethod_A"))
        {
            var error = new InvalidOperationException("SomeInnerInnerValidationMethod error A.");
            if (error.ShouldStop(ref scope))
                return scope;
        }

        if (HasError("SomeInnerInnerValidationMethod_B"))
        {
            var error = new InvalidOperationException("SomeInnerInnerValidationMethod error B.");
            if (error.ShouldStop(ref scope))
                return scope;
        }

        return scope;
    }

    private static ValidationResult<int> SomeValueValidationMethod(Scope scope = default)
    {
        if (HasError("SomeValueValidationMethod_A"))
        {
            var error = new InvalidOperationException("SomeValueValidationMethod error A.");
            if (error.ShouldStop(ref scope))
                return scope.ToResult<int>();
        }

        var valueResult = SomeInnerValueValidationMethod(scope.ToInner());
        valueResult.CaptureBy(ref scope);

        if (!valueResult.IsValid)
            return scope.ToResult<int>();

        var value = valueResult.Value!.Value;

        if (HasError("SomeValueValidationMethod_B"))
        {
            var error = new InvalidOperationException("SomeValueValidationMethod error B.");
            error.CaptureBy(ref scope);
            return scope.ToResult<int>();
        }

        var result = value * -1;

        return scope.ToResult(result);
    }

    private static ValidationResult<int?> SomeInnerValueValidationMethod(Scope scope = default)
    {
        if (HasError("SomeInnerValueValidationMethod_A"))
        {
            var error = new InvalidOperationException("SomeInnerValueValidationMethod error A.");
            error.CaptureBy(ref scope);
            return scope.ToResult<int?>();
        }

        return scope.ToResult<int?>(-12345);
    }

    private static bool HasError(string error) => _errors.Contains(error);
}
