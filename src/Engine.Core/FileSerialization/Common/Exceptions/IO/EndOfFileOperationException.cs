﻿using System;
using System.Diagnostics;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal sealed class EndOfFileOperationException : OperationException
{
    private new const string MessageTemplate = $"{OperationException.MessageTemplate}; end of file reached. Attempted to {{0}} {{2}} byte(s) beyond the file length.";

    public EndOfFileOperationException(OperationType operation, int bytesCount, long trailingBytesCount)
        : base(operation, bytesCount, ComposeMessage(operation, bytesCount, trailingBytesCount))
    {
        TrailingBytesCount = trailingBytesCount;
    }

    public EndOfFileOperationException(OperationType operation, int bytesCount, long trailingBytesCount, Exception inner)
        : base(operation, bytesCount, ComposeMessage(operation, bytesCount, trailingBytesCount), inner)
    {
        TrailingBytesCount = trailingBytesCount;
    }

    public long TrailingBytesCount { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public static IOperationExceptionFluentBuilderSelectOperationStage<EndOfFileOperationException> With =>
        new OperationExceptionFluentBuilder<EndOfFileOperationException>();

    private static string ComposeMessage(OperationType operation, int bytesCount, long trailingBytesCount) =>
        string.Format(MessageTemplate, GetMessageComponent(operation), bytesCount, trailingBytesCount);
}
