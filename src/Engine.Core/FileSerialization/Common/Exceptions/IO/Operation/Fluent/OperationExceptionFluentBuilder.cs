using System.Diagnostics;
using System.Text;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

internal record struct OperationExceptionFluentBuilder<TException> :
    IOperationExceptionFluentBuilderSelectOperationStage<TException>,
    IOperationExceptionFluentBuilder<TException>
    where TException : OperationException
{
    public OperationType Operation { get; private set; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IOperationExceptionFluentBuilder<TException> Read
    {
        get
        {
            Operation = OperationType.Read;
            return this;
        }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IOperationExceptionFluentBuilder<TException> ReadSkip
    {
        get
        {
            Operation = OperationType.ReadSkip;
            return this;
        }
    }

    private bool PrintMembers(StringBuilder stringBuilder)
    {
        stringBuilder.Append($"{nameof(TException)} = {typeof(TException).Name}, {nameof(Operation)} = {Operation}");
        return true;
    }
}
