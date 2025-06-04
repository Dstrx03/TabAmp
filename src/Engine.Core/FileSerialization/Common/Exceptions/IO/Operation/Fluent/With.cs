using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation;

internal partial class EndOfFileOperationException
{
    public abstract class With : OperationExceptionFluentBuilderSelectOperationStage<EndOfFileOperationException> { }
}

internal partial class NegativeBytesCountOperationException
{
    public abstract class With : OperationExceptionFluentBuilderSelectOperationStage<NegativeBytesCountOperationException> { }
}
