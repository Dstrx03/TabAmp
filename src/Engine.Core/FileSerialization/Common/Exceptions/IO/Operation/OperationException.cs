using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation;

internal abstract class OperationException : FileSerializationException
{
    protected const string MessageTemplate = "Unable to {0} the next {1} byte(s)";

    protected OperationException(Operation operation, int bytesCount, string message)
        : base(message)
    {
        Operation = operation;
        BytesCount = bytesCount;
    }

    protected OperationException(Operation operation, int bytesCount, string message, Exception inner)
        : base(message, inner)
    {
        Operation = operation;
        BytesCount = bytesCount;
    }

    public Operation Operation { get; }
    public int BytesCount { get; }

    protected static string GetMessageComponent(Operation operation) => operation switch
    {
        Operation.Read => "read",
        Operation.ReadSkip => "read (skip)",
        _ => $"perform an unidentified operation ({nameof(Operation)}: {(int)operation}) on"
    };
}
