using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IntegrityValidation.Fluent;

internal static class Ensure
{
    public static A<T> That<T>(T value, [CallerArgumentExpression(nameof(value))] string? identifier = default)
    {
        return new A<T>(value, identifier);
    }

    public static A<T> ThatAnonymous<T>(T value)
    {
        return new A<T>(value, null);
    }
}

internal readonly ref struct A<T>
{
    private readonly T _value;
    private readonly string? _identifier;

    public A(T value, string? identifier)
    {
        _value = value;
        _identifier = identifier;
    }

    public B<T> WithLabel(string label) => new(_value, _identifier, label);

    public C<T> WithUnit(string unit) => new(_value, _identifier, null, unit);

    public ValidationContext<T> Is => new(_value, _identifier, null, null);
}

internal readonly ref struct B<T>
{
    private readonly T _value;
    private readonly string? _identifier;
    private readonly string? _label;

    public B(T value, string? identifier, string? label)
    {
        _value = value;
        _identifier = identifier;
        _label = label;
    }

    public C<T> WithUnit(string unit) => new(_value, _identifier, _label, unit);

    public ValidationContext<T> Is => new(_value, _identifier, _label, null);
}

internal readonly ref struct C<T>
{
    private readonly T _value;
    private readonly string? _identifier;
    private readonly string? _label;
    private readonly string? _unit;

    public C(T value, string? identifier, string? label, string? unit)
    {
        _value = value;
        _identifier = identifier;
        _label = label;
        _unit = unit;
    }

    public ValidationContext<T> Is => new(_value, _identifier, _label, _unit);
}

internal readonly ref struct ValidationContext<T>
{
    private readonly T _value;
    private readonly string? _identifier;
    private readonly string? _label;
    private readonly string? _unit;

    public ValidationContext(T value, string? identifier, string? label, string? unit)
    {
        _value = value;
        _identifier = identifier;
        _label = label;
        _unit = unit;
    }

    public ValidationFailure Apply<TRule>(TRule rule) where TRule : struct, IValidationRule<T>, allows ref struct
    {
        return rule.Validate(_value, _identifier, _label, _unit);
    }
}

internal static class ValidationContextExtensions
{
    public static ValidationFailure EqualTo<T>(this ValidationContext<T> context, T expected) where T : IEquatable<T>
    {
        return context.Apply(new IsEqualTo<T>(expected));
    }
}

internal interface IValidationRule<T>
{
    ValidationFailure Validate(T value, string? identifier, string? label, string? unit);
}

internal readonly ref struct IsEqualTo<T> : IValidationRule<T> where T : IEquatable<T>
{
    private const string MessageTemplate = "The {0} is expected to be {1}. Actual {2}: {3}.";

    private readonly T _expected;

    public IsEqualTo(T expected)
    {
        _expected = expected;
    }

    public ValidationFailure Validate(T value, string? identifier, string? label, string? unit)
    {
        if (value.Equals(_expected))
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

internal readonly ref struct ValidationFailure
{
    public ValidationFailure(string message)
    {
        Message = message;
    }

    public string Message { get; }
    public bool HasFailure => Message is not null;

    public T Build<T>() where T : Exception
    {
        /*var info = typeof(T).GetConstructor([typeof(string)]);
        var instance = info!.Invoke([Message]);
        return (T)instance;*/

        var assembly = typeof(ExceptionBuilder).Assembly;

        var builders = assembly.GetTypes().Where(type => type.GetInterfaces().Any(i =>
           i.IsGenericType &&
           i.GetGenericTypeDefinition() == typeof(IExceptionBuilder<>) &&
           i.GenericTypeArguments[0] == typeof(T)));

        var builder = builders.FirstOrDefault();

        if (builder is null)
            throw new InvalidOperationException($"TODO: {nameof(IExceptionBuilder<T>)}:{typeof(T).Name} not found");

        /*var ctorInfo = builder.GetConstructor(Type.EmptyTypes);
        var builderInstance = ctorInfo.Invoke(null) as IExceptionBuilder<T>;*/

        var builderInstance = Activator.CreateInstance(builder) as IExceptionBuilder<T>;

        var exc = builderInstance!.Build(Message);

        return exc;
    }

    public void Throw<T>() where T : Exception
    {
        if (HasFailure)
            throw Build<T>();
    }
}

internal interface IExceptionBuilder<T> where T : Exception
{
    T Build(string message);
}

internal class ExceptionBuilder : IExceptionBuilder<ProcessIntegrityException>,
    IExceptionBuilder<ArgumentNullException>
{
    ProcessIntegrityException IExceptionBuilder<ProcessIntegrityException>.Build(string message)
    {
        return new ProcessIntegrityException(message);
    }

    ArgumentNullException IExceptionBuilder<ArgumentNullException>.Build(string message)
    {
        return new ArgumentNullException("test param name", message);
    }
}

internal class ProcessIntegrityExceptionFluentBuilder
{
    private void FooBar(int a, [CallerArgumentExpression(nameof(a))] string? aName = default) { }
}
