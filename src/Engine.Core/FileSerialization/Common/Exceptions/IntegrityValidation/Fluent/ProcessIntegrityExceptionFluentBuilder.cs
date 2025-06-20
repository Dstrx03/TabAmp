using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IntegrityValidation.Fluent;

internal static class Ensure
{
    public static A<T> That<T>(T value, [CallerArgumentExpression(nameof(value))] string? valueName = default)
    {
        return new A<T>(value, valueName);
    }
}

internal struct A<T>
{
    private T _value;
    private string? _valueName;

    public A(T value, string? valueName)
    {
        _value = value;
        _valueName = valueName;
    }

    public ValidationContext<T> Is => new(_value);
}

internal struct ValidationContext<T>
{
    private T _value;

    public ValidationContext(T value)
    {
        _value = value;
    }

    public ValidationFailure Apply<TRule>(TRule rule) where TRule : struct, IValidationRule<T>
    {
        return rule.Validate(_value);
    }
}

internal static class ValidationContextExtensions
{
    public static ValidationFailure EqualTo<T>(this ValidationContext<T> context, T expected)
    {
        return context.Apply(new IsEqualTo<T>(expected));
    }
}

internal interface IValidationRule<T>
{
    ValidationFailure Validate(T value);
}

internal struct IsEqualTo<T> : IValidationRule<T>
{
    private T _expected;

    public IsEqualTo(T expected)
    {
        _expected = expected;
    }

    public ValidationFailure Validate(T value)
    {
        if (EqualityComparer<T>.Default.Equals(value, _expected))
            return default;

        var message = $"value: {value}, expected: {_expected}";
        return new(message);
    }
}

internal struct ValidationFailure
{
    public ValidationFailure(string message)
    {
        Message = message;
    }

    public string Message { get; }
    public bool HasFailure => Message is not null;
}

internal class ProcessIntegrityExceptionFluentBuilder
{
    private void FooBar(int a, [CallerArgumentExpression(nameof(a))] string? aName = default) { }
}
