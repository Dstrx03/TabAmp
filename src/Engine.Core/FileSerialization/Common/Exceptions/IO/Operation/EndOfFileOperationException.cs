using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation;

internal sealed partial class EndOfFileOperationException : OperationException
{
    private new const string MessageTemplate = $"{OperationException.MessageTemplate}; end of file reached. Attempted to {{0}} {{2}} byte(s) beyond the file length.";

    public EndOfFileOperationException(Operation operation, int bytesCount, long trailingBytesCount)
        : base(operation, bytesCount, ComposeMessage(operation, bytesCount, trailingBytesCount))
    {
        TrailingBytesCount = trailingBytesCount;
    }

    public EndOfFileOperationException(Operation operation, int bytesCount, long trailingBytesCount, Exception inner)
        : base(operation, bytesCount, ComposeMessage(operation, bytesCount, trailingBytesCount), inner)
    {
        TrailingBytesCount = trailingBytesCount;
    }

    public long TrailingBytesCount { get; }

    private static string ComposeMessage(Operation operation, int bytesCount, long trailingBytesCount) =>
        string.Format(MessageTemplate, GetMessageComponent(operation), bytesCount, trailingBytesCount);
}
