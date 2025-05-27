namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

internal struct OperationExceptionFluentBuilder<TException>() :
    IOperationExceptionFluentBuilderSelectOperationStage<TException>,
    IOperationExceptionFluentBuilder<TException>
    where TException : OperationException
{
    public Operation Operation { get; private set; } = (Operation)(-1);

    public IOperationExceptionFluentBuilder<TException> SelectOperation(Operation operation)
    {
        Operation = operation;
        return this;
    }
}
