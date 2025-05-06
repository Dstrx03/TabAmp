namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

internal interface IOperationExceptionFluentBuilder<TException>
    where TException : OperationException
{
    OperationType Operation { get; }
}
