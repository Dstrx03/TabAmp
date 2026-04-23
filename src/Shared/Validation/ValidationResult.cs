using System.Diagnostics;
using TabAmp.Shared.Validation.Exceptions;

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

    public void ThrowIfAnyErrors(string? message = null)
    {
        if (IsValid)
            return;

        throw new ValidationException(message, Errors.ToList());
    }
}
