using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal abstract class OperationException : FileSerializationException
{
    protected const string MessageTemplate = "Unable to {0} the next {1} byte(s)";

    protected OperationException(OperationType operation, int byteCount, string message)
        : base(message)
    {
        Operation = operation;
        ByteCount = byteCount;
    }

    protected OperationException(OperationType operation, int byteCount, string message, Exception inner)
        : base(message, inner)
    {
        Operation = operation;
        ByteCount = byteCount;
    }

    public OperationType Operation { get; }
    public int ByteCount { get; }

    protected static string GetMessageComponent(OperationType operation) => operation switch
    {
        OperationType.Read => "read",
        OperationType.ReadSkip => "read (skip)",
        _ => $"perform an unidentified operation (type: {(int)operation}) on"
    };
}
