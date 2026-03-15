using System;

namespace TabAmp.Shared.Validation;

public readonly ref struct Scope
{
    private readonly Context _context;
    private readonly Exception? _error;

    private Scope(Context context, Exception? error)
    {
        _context = context;
        _error = error;
    }

    public bool StopOnFirstError => _context.StopOnFirstError;

    internal Scope WithError(Exception error)
    {
        var context = _context.WithError(error);
        var scopeError = _context.StopOnFirstError ? error : null;

        return new(context, scopeError);
    }

    internal ValidationResult ToResult() => _error is not null ? new(_error) : _context.ToResult();

    public static Scope Init_TODONAME(bool stopOnFirstError) =>
        new(Context.Init_TODONAME(stopOnFirstError: stopOnFirstError), error: null);

    public static implicit operator ValidationResult(Scope scope) => scope.ToResult();
}
