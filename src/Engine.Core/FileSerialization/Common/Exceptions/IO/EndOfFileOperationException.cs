using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal sealed class EndOfFileOperationException : OperationException
{
    private new const string MessageTemplate = $"{OperationException.MessageTemplate}; end of file reached. Attempted to {{0}} {{2}} byte(s) beyond the file length.";

    public EndOfFileOperationException(OperationType operation, int bytesCount, int trailingBytesCount)
        : base(operation, bytesCount, ComposeMessage(operation, bytesCount, trailingBytesCount))
    {
        TrailingBytesCount = trailingBytesCount;
    }

    public EndOfFileOperationException(OperationType operation, int bytesCount, int trailingBytesCount, Exception inner)
        : base(operation, bytesCount, ComposeMessage(operation, bytesCount, trailingBytesCount), inner)
    {
        TrailingBytesCount = trailingBytesCount;
    }

    public int TrailingBytesCount { get; }

    private static string ComposeMessage(OperationType operation, int bytesCount, int trailingBytesCount) =>
        string.Format(MessageTemplate, GetMessageComponent(operation), bytesCount, trailingBytesCount);
}
