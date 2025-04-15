namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Fluent;

internal record struct OperationExceptionFluentBuilder<TException> :
    IOperationExceptionFluentBuilderSelectOperationStage<TException>,
    IOperationExceptionFluentBuilder<TException>
    where TException : OperationException
{
    public OperationType Operation { get; private set; }

    public IOperationExceptionFluentBuilder<TException> Read
    {
        get
        {
            Operation = OperationType.Read;
            return this;
        }
    }

    public IOperationExceptionFluentBuilder<TException> ReadSkip
    {
        get
        {
            Operation = OperationType.ReadSkip;
            return this;
        }
    }

    public override string ToString() =>
        $"{nameof(OperationExceptionFluentBuilder<TException>)} {{ TException = {typeof(TException).Name}, Operation = {Operation} }}";
}
