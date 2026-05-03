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

        var valueResult = ValueValidationMethod(scope.ToInner());
        valueResult.CaptureBy(ref scope);
        var value3 = valueResult.Value;
        var err3 = valueResult.Errors.ToList();

        var noErrorsResult = NoErrorsValidationMethod(scope.ToInner());
        noErrorsResult.CaptureBy(ref scope);
        var err4 = noErrorsResult.Errors.ToList();

        var valueWithErrorResult = ValueWithErrorValidationMethod(scope.ToInner());
        valueWithErrorResult.CaptureBy(ref scope);
        var value5 = valueWithErrorResult.Value;
        var err5 = valueWithErrorResult.Errors.ToList();

        var nullableValueWithErrorResult = NullableValueWithErrorValidationMethod(scope.ToInner());
        nullableValueWithErrorResult.CaptureBy(ref scope);
        var value6 = nullableValueWithErrorResult.Value;
        var err6 = nullableValueWithErrorResult.Errors.ToList();

        var stringResult = StringValidationMethod(scope.ToInner());
        stringResult.CaptureBy(ref scope);
        var value7 = stringResult.Value;
        var err7 = stringResult.Errors.ToList();

        var stringWithErrorResult = StringWithErrorValidationMethod(scope.ToInner());
        stringWithErrorResult.CaptureBy(ref scope);
        var value8 = stringWithErrorResult.Value;
        var err8 = stringWithErrorResult.Errors.ToList();

        var nullableStringWithErrorResult = NullableStringWithErrorValidationMethod(scope.ToInner());
        nullableStringWithErrorResult.CaptureBy(ref scope);
        var value9 = nullableStringWithErrorResult.Value;
        var err9 = nullableStringWithErrorResult.Errors.ToList();

        var doubleErrorsResult = DoubleErrorsValidationMethod(scope.ToInner());
        doubleErrorsResult.CaptureBy(ref scope);
        var err10 = doubleErrorsResult.Errors.ToList();

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

    private static ValidationResult<int> ValueValidationMethod(Scope scope = default)
    {
        return scope.ToResult(12345);
    }

    private static ValidationResult<int> ValueWithErrorValidationMethod(Scope scope = default)
    {
        new InvalidOperationException("some value error").CaptureBy(ref scope);

        return scope.ToResult<int>();
    }

    private static ValidationResult<int?> NullableValueWithErrorValidationMethod(Scope scope = default)
    {
        new InvalidOperationException("some nullable value error").CaptureBy(ref scope);

        return scope.ToResult<int?>();
    }

    private static ValidationResult<string> StringValidationMethod(Scope scope = default)
    {
        return scope.ToResult("I am a result.");
    }

    private static ValidationResult<string> StringWithErrorValidationMethod(Scope scope = default)
    {
        new InvalidOperationException("some string error").CaptureBy(ref scope);

        return scope.ToResult<string>();
    }

    private static ValidationResult<string?> NullableStringWithErrorValidationMethod(Scope scope = default)
    {
        new InvalidOperationException("some nullable string error").CaptureBy(ref scope);

        return scope.ToResult<string?>();
    }
}
