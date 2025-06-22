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

    public C<T> WithUnit(string unit) => new(_value, _identifier, null, unit);

    public ValidationContext<T> Is => new(_value, _identifier, null, null);
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

    public C<T> WithUnit(string unit) => new(_value, _identifier, _label, unit);

    public ValidationContext<T> Is => new(_value, _identifier, _label, null);
}

internal struct C<T>
{
    private T _value;
    private string? _identifier;
    private string? _label;
    private string? _unit;

    public C(T value, string? identifier, string? label, string? unit)
    {
        _value = value;
        _identifier = identifier;
        _label = label;
        _unit = unit;
    }

    public ValidationContext<T> Is => new(_value, _identifier, _label, _unit);
}

internal struct ValidationContext<T>
{
    private T _value;
    private string? _identifier;
    private string? _label;
    private string? _unit;

    public ValidationContext(T value, string? identifier, string? label, string? unit)
    {
        _value = value;
        _identifier = identifier;
        _label = label;
        _unit = unit;
    }

    public ValidationFailure Apply<TRule>(TRule rule) where TRule : struct, IValidationRule<T>
    {
        return rule.Validate(_value, _identifier, _label, _unit);
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
    ValidationFailure Validate(T value, string? identifier, string? label, string? unit);
}

internal struct IsEqualTo<T> : IValidationRule<T>
{
    private const string MessageTemplate = "The {0} is expected to be {1}. Actual {2}: {3}.";

    private T _expected;

    public IsEqualTo(T expected)
    {
        _expected = expected;
    }

    public ValidationFailure Validate(T value, string? identifier, string? label, string? unit)
    {
        if (EqualityComparer<T>.Default.Equals(value, _expected))
            return default;

        return new(ComposeMessage(value, identifier, label, unit));
    }

    private string ComposeMessage(T value, string? identifier, string? label, string? unit) =>
        string.Format(MessageTemplate,
            GetLabelMessageComponent(identifier, label),
            GetValueMessageComponent(_expected, unit),
            GetSummaryLabelMessageComponent(label, unit),
            GetValueMessageComponent(value, unit));

    private string GetLabelMessageComponent(string? identifier, string? label) => (identifier, label) switch
    {
        (null, null) => "value",
        (string identifierToken, null) => identifierToken,
        (null, string labelToken) => labelToken,
        _ => $"{label} {identifier}"
    };

    private string GetSummaryLabelMessageComponent(string? label, string? unit) =>
        unit is null ? GetLabelMessageComponent(null, null) : GetLabelMessageComponent(null, label);

    private string GetValueMessageComponent(T value, string? unit) =>
        unit is null ? $"{value}" : $"{value} {unit}";
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
