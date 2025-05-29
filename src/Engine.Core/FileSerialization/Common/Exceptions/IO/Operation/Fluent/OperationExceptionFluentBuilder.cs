using System.Diagnostics;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

internal class OperationExceptionFluentBuilderSelectOperationStage<TException>
    where TException : OperationException
{
    public static readonly OperationExceptionFluentBuilderSelectOperationStage<TException> With = new();

    private OperationExceptionFluentBuilderSelectOperationStage()
    {
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public OperationExceptionFluentBuilder<TException> Read => new(Operation.Read);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public OperationExceptionFluentBuilder<TException> ReadSkip => new(Operation.ReadSkip);
}

internal readonly record struct OperationExceptionFluentBuilder<TException>(Operation Operation)
    where TException : OperationException;
