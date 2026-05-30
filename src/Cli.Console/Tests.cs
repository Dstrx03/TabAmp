using System;
using System.Collections.Generic;
using System.Linq;
using TabAmp.Shared.Fuse;
using TabAmp.Shared.Fuse.Extensions;

namespace TabAmp.Cli.Console;

internal static class Tests
{
    private static readonly HashSet<string> _errorsAll = [
        "SomeValidationMethod_A",
        "SomeInnerValidationMethod_A",
        "SomeInnerInnerValidationMethod_A",
        "SomeInnerInnerValidationMethod_B",
        "SomeValueValidationMethod_A",
        "SomeValueValidationMethod_B",
        "SomeInnerValueValidationMethod_A"
    ];

    private static HashSet<string> _errors = null!;

    public static void Run()
    {
        var stopOnFirstError = false;

        RunNormal(stopOnFirstError);
        RunExhaustive(stopOnFirstError);
    }

    private static void RunNormal(bool stopOnFirstError)
    {
        System.Console.WriteLine("\nNORMAL RUN: START");

        _errors = _errorsAll;
        var result = SomeValidationMethod(new(stopOnFirstError: stopOnFirstError));
        var valueResult = SomeValueValidationMethod(new(stopOnFirstError: stopOnFirstError));

        System.Console.WriteLine($"\nresult: {result.IsSuccess} (stopOnFirstError: {stopOnFirstError})");
        foreach (var error in result.Errors)
            System.Console.WriteLine($" - {error.Message}");

        try
        {
            result.ThrowIfAnyErrors();
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"\nCATCH {e}");
        }

        System.Console.WriteLine($"\nvalueResult: {valueResult.IsSuccess}, '{valueResult.Value}' (stopOnFirstError: {stopOnFirstError})");
        foreach (var error in valueResult.Errors)
            System.Console.WriteLine($" - {error.Message}");

        try
        {
            valueResult.ThrowIfAnyErrors();
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"\nCATCH {e}");
        }

        System.Console.WriteLine("\nNORMAL RUN: OK");
    }

    private static void RunExhaustive(bool stopOnFirstError)
    {
        System.Console.WriteLine("\nEXHAUSTIVE RUN: START");

        foreach (var errors in GetErrorsExhaustive(_errorsAll))
        {
            _errors = errors;
            SomeValidationMethod(new(stopOnFirstError: stopOnFirstError));
            SomeValueValidationMethod(new(stopOnFirstError: stopOnFirstError));
        }

        System.Console.WriteLine("EXHAUSTIVE RUN: OK");
    }

    private static FuseResult SomeValidationMethod(FuseScope scope = default)
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

    private static FuseResult SomeInnerValidationMethod(FuseScope scope = default)
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

    private static FuseResult SomeInnerInnerValidationMethod(FuseScope scope = default)
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

    private static FuseResult<int> SomeValueValidationMethod(FuseScope scope = default)
    {
        if (HasError("SomeValueValidationMethod_A"))
        {
            var error = new InvalidOperationException("SomeValueValidationMethod error A.");
            if (error.ShouldStop(ref scope))
                return scope.ToResult<int>();
        }

        var valueResult = SomeInnerValueValidationMethod(scope.ToInner());
        valueResult.CaptureBy(ref scope);

        if (!valueResult.IsSuccess)
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

    private static FuseResult<int?> SomeInnerValueValidationMethod(FuseScope scope = default)
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

    public static IEnumerable<HashSet<T>> GetErrorsExhaustive<T>(HashSet<T> source)
    {
        var items = source.ToArray();
        var count = items.Length;

        for (var mask = 0; mask < (1 << count); mask++)
        {
            var combination = new HashSet<T>();

            for (var i = 0; i < count; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    combination.Add(items[i]);
                }
            }

            yield return combination;
        }
    }
}
