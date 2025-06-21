using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IntegrityValidation.Fluent;

internal static class Ensure
{
    public static A<T> That<T>(T value, [CallerArgumentExpression(nameof(value))] string? identifier = default)
    {
        return new A<T>(value, identifier);
    }

    public static A<T> That_Todo<T>(T value)
    {
        return new A<T>(value, null);
    }
}

internal struct A<T>
{
    private T _value;
    private string? _identifier;

    public A(T value, string? identifier)
    {
        _value = value;
        _identifier = identifier;
    }

    public B<T> WithLabel(string label) => new(_value, _identifier, label);

    public ValidationContext<T> Is => new(_value, _identifier, null);
}

internal struct B<T>
{
    private T _value;
    private string? _identifier;
    private string? _label;

    public B(T value, string? identifier, string? label)
    {
        _value = value;
        _identifier = identifier;
        _label = label;
    }

    public ValidationContext<T> Is => new(_value, _identifier, _label);
}

internal struct ValidationContext<T>
{
    private T _value;
    private string? _identifier;
    private string? _label;

    public ValidationContext(T value, string? identifier, string? label)
    {
        _value = value;
        _identifier = identifier;
        _label = label;
    }

    public ValidationFailure Apply<TRule>(TRule rule) where TRule : struct, IValidationRule<T>
    {
        return rule.Validate(_value, _identifier, _label);
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
    ValidationFailure Validate(T value, string? identifier, string? label);
}

internal struct IsEqualTo<T> : IValidationRule<T>
{
    private T _expected;

    public IsEqualTo(T expected)
    {
        _expected = expected;
    }

    public ValidationFailure Validate(T value, string? identifier, string? label)
    {
        if (EqualityComparer<T>.Default.Equals(value, _expected))
            return default;

        var message = $"The {GetLabelToken(identifier, label)} is expected to be {GetValueToken(_expected)}. Actual value: {GetValueToken(value)}.";
        return new(message);
    }

    private string GetLabelToken(string? identifier, string? label) => (identifier, label) switch
    {
        (null, null) => "value",
        (string identifierToken, null) => identifierToken,
        (null, string labelToken) => labelToken,
        _ => $"{label} {identifier}"
    };

    private string GetValueToken(T value)
    {
        return value.ToString();
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
