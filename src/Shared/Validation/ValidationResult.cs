using TabAmp.Shared.Validation.Exceptions;

namespace TabAmp.Shared.Validation;

public readonly ref struct ValidationResult
{
    private readonly Context _context;

    internal ValidationResult(Context context) => _context = context;

    public Errors Errors => _context.Errors;
    public bool IsValid => Errors.IsEmpty;

    public void ThrowIfAnyErrors(string? message = null)
    {
        if (IsValid)
            return;

        throw new ValidationException(message, Errors.ToList());
    }
}
