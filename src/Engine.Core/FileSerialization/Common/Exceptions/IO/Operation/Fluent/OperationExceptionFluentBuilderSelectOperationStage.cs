using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation;

internal abstract class OperationExceptionFluentBuilderSelectOperationStage<TException>
    where TException : OperationException
{
    public static OperationExceptionFluentBuilder<TException> Read => new(Operation.Read);
    public static OperationExceptionFluentBuilder<TException> ReadSkip => new(Operation.ReadSkip);
}

internal partial class EndOfFileOperationException
{
    public abstract class With : OperationExceptionFluentBuilderSelectOperationStage<EndOfFileOperationException> { }
}

internal partial class NegativeBytesCountOperationException
{
    public abstract class With : OperationExceptionFluentBuilderSelectOperationStage<NegativeBytesCountOperationException> { }
}
