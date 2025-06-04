using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation;

internal sealed partial class NegativeBytesCountOperationException : OperationException
{
    private new const string MessageTemplate = $"{OperationException.MessageTemplate}; the specified byte count must be a non-negative number.";

    public NegativeBytesCountOperationException(Operation operation, int bytesCount)
        : base(operation, bytesCount, ComposeMessage(operation, bytesCount))
    {
    }

    public NegativeBytesCountOperationException(Operation operation, int bytesCount, Exception inner)
        : base(operation, bytesCount, ComposeMessage(operation, bytesCount), inner)
    {
    }

    private static string ComposeMessage(Operation operation, int bytesCount) =>
        string.Format(MessageTemplate, GetMessageComponent(operation), bytesCount);
}
