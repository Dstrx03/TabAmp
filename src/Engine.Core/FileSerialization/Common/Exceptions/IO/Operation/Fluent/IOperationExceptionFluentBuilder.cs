namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

internal interface IOperationExceptionFluentBuilderSelectOperationStage<TException>
    where TException : OperationException
{
    IOperationExceptionFluentBuilder<TException> Read => SelectOperation(Operation.Read);
    IOperationExceptionFluentBuilder<TException> ReadSkip => SelectOperation(Operation.ReadSkip);

    protected IOperationExceptionFluentBuilder<TException> SelectOperation(Operation operation);
}

internal interface IOperationExceptionFluentBuilder<TException>
    where TException : OperationException
{
    Operation Operation { get; }
}
