using System;
using System.Diagnostics;

namespace TabAmp.Shared.Fuse;

[DebuggerDisplay("ShouldStop = {ShouldStop}")]
public readonly ref struct FuseScope
{
    internal FuseContext Context { get; }

    public FuseScope(bool stopOnFirstError) : this(new FuseContext(stopOnFirstError: stopOnFirstError))
    {
    }

    private FuseScope(FuseContext context) => Context = context;

    public bool ShouldStop => Context.StopOnFirstError && !Context.Errors.IsEmpty;

    internal FuseScope With(Exception error) => new(Context.With(error));

    public FuseScope ToInner() => new(Context.ToInner());
    internal FuseScope FromOuter(FuseScope outer) => new(Context.FromOuter(outer.Context));

    public FuseResult ToResult() => new(this);
    public FuseResult<TValue> ToResult<TValue>() => new(ToResult(), default!);
    public FuseResult<TValue> ToResult<TValue>(TValue value) => new(ToResult(), value);

    public static implicit operator FuseResult(FuseScope scope) => scope.ToResult();
}
