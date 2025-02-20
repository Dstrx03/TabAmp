using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal abstract class OperationException : FileSerializationException
{
    protected const string MessageTemplate = "Unable to {0} the next {1} byte(s)";

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
