using System;
using System.Diagnostics;

namespace TabAmp.Shared.Validation;

[DebuggerDisplay("ShouldStop = {ShouldStop}")]
public readonly ref struct Scope
{
    internal Context Context { get; }

    public Scope(bool stopOnFirstError) : this(new Context(stopOnFirstError: stopOnFirstError))
    {
    }

    private Scope(Context context) => Context = context;

    public bool ShouldStop => Context.StopOnFirstError && !Context.Errors.IsEmpty;

    internal Scope With(Exception error) => new(Context.With(error));

    public Scope ToInner() => new(Context.ToInner());
    internal Scope FromOuter(Scope outer) => new(Context.FromOuter(outer.Context));

    public ValidationResult ToResult() => new(this);
    public ValidationResult<TValue> ToResult<TValue>() => new(ToResult(), default!);
    public ValidationResult<TValue> ToResult<TValue>(TValue value) => new(ToResult(), value);
}
