using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal abstract partial class OperationException : FileSerializationException
{
    internal sealed class As
    {
        private As() { }

        public static IOperationExceptionFluentBuilderSelectOperationStage<NegativeBytesCountOperationException> NegativeBytesCount =>
            new OperationExceptionFluentBuilder<NegativeBytesCountOperationException>();

        public static IOperationExceptionFluentBuilderSelectOperationStage<EndOfFileOperationException> EndOfFile =>
            new OperationExceptionFluentBuilder<EndOfFileOperationException>();
    }
}

