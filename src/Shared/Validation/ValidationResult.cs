using System.Diagnostics;
using TabAmp.Shared.Validation.Exceptions;
using TabAmp.Shared.Validation.Formatters;

namespace TabAmp.Shared.Validation;

[DebuggerDisplay("IsValid = {IsValid}")]
public readonly ref struct ValidationResult
{
    private readonly Scope _scope;

    internal ValidationResult(Scope scope) => _scope = scope;

    public Errors Errors => _scope.Context.Errors;
    public bool IsValid => Errors.IsEmpty;

    public Scope CaptureBy(ref Scope outer)
    {
        outer = _scope.FromOuter(outer);
        return outer;
    }

    public bool ShouldStop(ref Scope outer) => CaptureBy(ref outer).ShouldStop;

    public void ThrowIfAnyErrors(string? message = null) =>
        ThrowIfAnyErrors(new MultilineValidationExceptionMessageFormatter(message));

    public void ThrowIfAnyErrors<TMessageFormatter>(TMessageFormatter messageFormatter)
        where TMessageFormatter : IValidationExceptionMessageFormatter, allows ref struct
    {
        if (IsValid)
            return;

        throw new ValidationException(messageFormatter.Format(Errors), Errors.ToList());
    }
}

[DebuggerDisplay("IsValid = {IsValid}, Value = {Value}")]
public readonly ref struct ValidationResult<TValue>
{
    private readonly ValidationResult _result;

    public TValue Value { get; }

    internal ValidationResult(ValidationResult result, TValue value)
    {
        _result = result;
        Value = value;
    }

    public Errors Errors => _result.Errors;
    public bool IsValid => _result.IsValid;

    public Scope CaptureBy(ref Scope outer) => _result.CaptureBy(ref outer);

    public bool ShouldStop(ref Scope outer) => _result.ShouldStop(ref outer);

    public void ThrowIfAnyErrors(string? message = null) => _result.ThrowIfAnyErrors(message);

    public void ThrowIfAnyErrors<TMessageFormatter>(TMessageFormatter messageFormatter)
        where TMessageFormatter : IValidationExceptionMessageFormatter, allows ref struct
    {
        _result.ThrowIfAnyErrors(messageFormatter);
    }
}
