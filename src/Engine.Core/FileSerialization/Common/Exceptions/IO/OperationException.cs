using System;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal abstract class OperationException : FileSerializationException
{
    protected const string MessageTemplate = "Unable to {0} the next {1} byte(s)";

    protected OperationException(OperationType operation, int bytesCount, string message)
        : base(message)
    {
        Operation = operation;
        BytesCount = bytesCount;
    }

    protected OperationException(OperationType operation, int bytesCount, string message, Exception inner)
        : base(message, inner)
    {
        Operation = operation;
        BytesCount = bytesCount;
    }

    public OperationType Operation { get; }
    public int BytesCount { get; }

    public static IOperationExceptionFluentBuilderSelectOperationStage<NegativeBytesCountOperationException> NegativeBytesCount =>
        new OperationExceptionFluentBuilder<NegativeBytesCountOperationException>();

    public static IOperationExceptionFluentBuilderSelectOperationStage<EndOfFileOperationException> EndOfFile =>
        new OperationExceptionFluentBuilder<EndOfFileOperationException>();

    protected static string GetMessageComponent(OperationType operation) => operation switch
    {
        OperationType.Read => "read",
        OperationType.ReadSkip => "read (skip)",
        _ => $"perform an unidentified operation (type: {(int)operation}) on"
    };
}
