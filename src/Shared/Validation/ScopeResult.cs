namespace TabAmp.Shared.Validation;

public readonly ref struct ScopeResult
{
    private readonly Scope _scope;

    private ScopeResult(Scope scope)
    {
        _scope = scope;
    }

    public bool WithError_TODONAME(ref Scope outerScope)
    {
        outerScope = _scope.ToOuterScope(outerScope);
        return outerScope.StopOnFirstError;
    }

    internal static ScopeResult FromScope(Scope scope) => new(scope);
}
