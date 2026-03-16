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

    internal Context ToContext() => _context;
    internal Scope ToOuterScope(Scope outerScope) => new(_context, _error);
    internal ScopeResult ToScopeResult() => ScopeResult.FromScope(this);
    internal ValidationResult ToResult() => _error is not null ? new(_error) : _context.ToResult();

    public static implicit operator Context(Scope scope) => scope.ToContext();
    public static implicit operator ScopeResult(Scope scope) => scope.ToScopeResult();
    public static implicit operator ValidationResult(Scope scope) => scope.ToResult();

    public static Scope Init_TODONAME(bool stopOnFirstError) =>
        new(Context.Init_TODONAME(stopOnFirstError: stopOnFirstError), error: null);

    internal static Scope FromContext(Context context) => new(context, error: null);
}
