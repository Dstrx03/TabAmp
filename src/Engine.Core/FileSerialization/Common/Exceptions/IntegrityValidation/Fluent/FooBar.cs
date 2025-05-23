using System.Runtime.CompilerServices;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IntegrityValidation.Fluent;

internal interface A
{
    B AnonymousProperty(int value, [CallerArgumentExpression(nameof(value))] string? valueName = default);
}
internal interface B
{
    C IsExpectedToBe(int expectedValue);
}
internal interface C
{
    void Throw();
}
internal class FooBar : A, B, C
{
    private int _value;
    private string _valueName;
    private string _valueDescription;

    private int _expectedValue;
    private bool _assertion;

    public B AnonymousProperty(int value, string? valueName)
    {
        _value = value;
        _valueName = valueName;
        _valueDescription = "anonymous property";
        return this;
    }

    public C IsExpectedToBe(int expectedValue)
    {
        _expectedValue = expectedValue;
        _assertion = _value != expectedValue;
        return this;
    }

    public void Throw()
    {
        if (!_assertion)
            return;

        var message = $"The {_valueDescription} {_valueName} is expected to be {_expectedValue}. Actual value: {_value}.";
        var exception = new ProcessIntegrityException(message);
        throw exception;
    }
}
