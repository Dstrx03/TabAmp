using System.Diagnostics;
using System.Text;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

internal record struct OperationExceptionFluentBuilder<TException>() :
    IOperationExceptionFluentBuilderSelectOperationStage<TException>,
    IOperationExceptionFluentBuilder<TException>
    where TException : OperationException
{
    public Operation Operation { get; private set; } = (Operation)(-1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IOperationExceptionFluentBuilder<TException> Read => SelectOperation(Operation.Read);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IOperationExceptionFluentBuilder<TException> ReadSkip => SelectOperation(Operation.ReadSkip);

    private OperationExceptionFluentBuilder<TException> SelectOperation(Operation operation)
    {
        Operation = operation;
        return this;
    }

    private bool PrintMembers(StringBuilder stringBuilder)
    {
        stringBuilder.Append($"{nameof(TException)} = {typeof(TException).Name}, {nameof(Operation)} = {Operation}");
        return true;
    }
}
