using System;

namespace TabAmp.Shared.Validation;

public readonly ref struct Scope
{
    private readonly Context _context;

    private Scope(Context context) => _context = context;

    public bool ShouldStop => _context.StopOnFirstError && !_context.Errors.IsEmpty;

    internal Scope With(Exception error) => new(_context.With(error));

    public Scope ToInner() => new(_context.ToInner());
    public ValidationResult ToResult() => new(_context);
}
