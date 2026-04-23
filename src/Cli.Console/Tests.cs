using System;
using TabAmp.Shared.Validation;
using TabAmp.Shared.Validation.Extensions;

namespace TabAmp.Cli.Console;

internal static class Tests
{
    public static void Run()
    {
        var result = SomeValidationMethod();
        var err = result.Errors.ToList();
    }

    private static ValidationResult SomeValidationMethod(Scope scope = default)
    {
        new InvalidOperationException("some error").CaptureBy(ref scope);
        var err1 = scope.ToResult().Errors.ToList();

        var result = InnerValidationMethod(scope.ToInner());
        result.CaptureBy(ref scope);
        var err2 = result.Errors.ToList();

        var noErrorsResult = NoErrorsValidationMethod(scope.ToInner());
        noErrorsResult.CaptureBy(ref scope);
        var err3 = noErrorsResult.Errors.ToList();

        var doubleErrorsResult = DoubleErrorsValidationMethod(scope.ToInner());
        doubleErrorsResult.CaptureBy(ref scope);
        var err4 = doubleErrorsResult.Errors.ToList();

        return scope.ToResult();
    }

    private static ValidationResult InnerValidationMethod(Scope scope = default)
    {
        new InvalidOperationException("some inner error").CaptureBy(ref scope);

        return scope.ToResult();
    }

    private static ValidationResult NoErrorsValidationMethod(Scope scope = default)
    {
        return scope.ToResult();
    }

    private static ValidationResult DoubleErrorsValidationMethod(Scope scope = default)
    {
        new InvalidOperationException("some double error A").CaptureBy(ref scope);
        new InvalidOperationException("some double error B").CaptureBy(ref scope);

        return scope.ToResult();
    }
}
