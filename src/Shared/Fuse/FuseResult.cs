using System.Diagnostics;
using TabAmp.Shared.Fuse.Exceptions;
using TabAmp.Shared.Fuse.Formatters;

namespace TabAmp.Shared.Fuse;

[DebuggerDisplay("IsSuccess = {IsSuccess}")]
public readonly ref struct FuseResult
{
    private readonly FuseScope _scope;

    internal FuseResult(FuseScope scope) => _scope = scope;

    public FuseErrors Errors => _scope.Context.Errors;
    public bool IsSuccess => Errors.IsEmpty;

    public FuseScope CaptureBy(ref FuseScope outer)
    {
        outer = _scope.FromOuter(outer);
        return outer;
    }

    public bool ShouldStop(ref FuseScope outer) => CaptureBy(ref outer).ShouldStop;

    public void ThrowIfAnyErrors(string? message = null) =>
        ThrowIfAnyErrors(new MultilineFuseFailureMessageFormatter(message));

    public void ThrowIfAnyErrors<TMessageFormatter>(TMessageFormatter messageFormatter)
        where TMessageFormatter : IFuseFailureMessageFormatter, allows ref struct
    {
        if (IsSuccess)
            return;

        throw new FuseFailureException(messageFormatter.Format(Errors), Errors.ToList());
    }
}

[DebuggerDisplay("IsSuccess = {IsSuccess}, Value = {Value}")]
public readonly ref struct FuseResult<TValue>
{
    private readonly FuseResult _result;

    public TValue Value { get; }

    internal FuseResult(FuseResult result, TValue value)
    {
        _result = result;
        Value = value;
    }

    public FuseErrors Errors => _result.Errors;
    public bool IsSuccess => _result.IsSuccess;

    public FuseScope CaptureBy(ref FuseScope outer) => _result.CaptureBy(ref outer);

    public bool ShouldStop(ref FuseScope outer) => _result.ShouldStop(ref outer);

    public void ThrowIfAnyErrors(string? message = null) => _result.ThrowIfAnyErrors(message);

    public void ThrowIfAnyErrors<TMessageFormatter>(TMessageFormatter messageFormatter)
        where TMessageFormatter : IFuseFailureMessageFormatter, allows ref struct
    {
        _result.ThrowIfAnyErrors(messageFormatter);
    }
}
