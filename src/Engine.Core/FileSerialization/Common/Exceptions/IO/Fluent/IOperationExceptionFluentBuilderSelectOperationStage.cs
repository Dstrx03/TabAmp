namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Fluent;

internal interface IOperationExceptionFluentBuilderSelectOperationStage<TException>
    where TException : OperationException
{
    IOperationExceptionFluentBuilder<TException> Read { get; }
    IOperationExceptionFluentBuilder<TException> ReadSkip { get; }
}
