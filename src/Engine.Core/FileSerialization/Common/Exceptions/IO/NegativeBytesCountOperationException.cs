using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal sealed class NegativeBytesCountOperationException : OperationException
{
    private new const string MessageTemplate = $"{OperationException.MessageTemplate}; the specified byte count must be a non-negative value.";

    public NegativeBytesCountOperationException(OperationType operation, int bytesCount)
        : base(operation, bytesCount, ComposeMessage(operation, bytesCount))
    {
    }

    public NegativeBytesCountOperationException(OperationType operation, int bytesCount, Exception inner)
        : base(operation, bytesCount, ComposeMessage(operation, bytesCount), inner)
    {
    }

    public static void ThrowIfNegative(OperationType operation, int bytesCount)
    {
        if (bytesCount < 0)
            throw new NegativeBytesCountOperationException(operation, bytesCount);
    }

    private static string ComposeMessage(OperationType operation, int bytesCount) =>
        string.Format(MessageTemplate, GetMessageComponent(operation), bytesCount);
}
