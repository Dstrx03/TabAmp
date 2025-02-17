using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal abstract class OperationException : FileSerializationException
{
    protected OperationException(OperationType operation, int count, string message)
        : base(message)
    {
        Operation = operation;
        Count = count;
    }

    protected OperationException(OperationType operation, int count, string message, Exception inner)
        : base(message, inner)
    {
        Operation = operation;
        Count = count;
    }

    public OperationType Operation { get; }
    public int Count { get; }
}
