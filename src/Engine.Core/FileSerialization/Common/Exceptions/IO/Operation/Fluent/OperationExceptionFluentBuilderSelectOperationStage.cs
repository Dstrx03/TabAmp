namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

internal abstract class OperationExceptionFluentBuilderSelectOperationStage<TException>
    where TException : OperationException
{
    public static OperationExceptionFluentBuilder<TException> Read => new(Operation.Read);
    public static OperationExceptionFluentBuilder<TException> ReadSkip => new(Operation.ReadSkip);
}
