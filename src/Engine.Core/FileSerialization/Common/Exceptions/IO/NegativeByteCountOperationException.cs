using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal sealed class NegativeByteCountOperationException : OperationException
{
    private new const string MessageTemplate = $"{OperationException.MessageTemplate}; the specified byte count must be a non-negative value.";

    public NegativeByteCountOperationException(OperationType operation, int byteCount)
        : base(operation, byteCount, ComposeMessage(operation, byteCount))
    {
    }

    public NegativeByteCountOperationException(OperationType operation, int byteCount, Exception inner)
        : base(operation, byteCount, ComposeMessage(operation, byteCount), inner)
    {
    }

    public static void ThrowIfNegative(OperationType operation, int byteCount)
    {
        if (byteCount < 0)
            throw new NegativeByteCountOperationException(operation, byteCount);
    }

    private static string ComposeMessage(OperationType operation, int byteCount) =>
        string.Format(MessageTemplate, GetMessageComponent(operation), byteCount);
}
