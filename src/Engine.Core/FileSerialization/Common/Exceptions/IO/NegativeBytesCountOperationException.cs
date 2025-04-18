﻿using System;
using System.Diagnostics;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal sealed class NegativeBytesCountOperationException : OperationException
{
    private new const string MessageTemplate = $"{OperationException.MessageTemplate}; the specified byte count must be a non-negative number.";

    public NegativeBytesCountOperationException(OperationType operation, int bytesCount)
        : base(operation, bytesCount, ComposeMessage(operation, bytesCount))
    {
    }

    public NegativeBytesCountOperationException(OperationType operation, int bytesCount, Exception inner)
        : base(operation, bytesCount, ComposeMessage(operation, bytesCount), inner)
    {
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public static IOperationExceptionFluentBuilderSelectOperationStage<NegativeBytesCountOperationException> With =>
        new OperationExceptionFluentBuilder<NegativeBytesCountOperationException>();

    private static string ComposeMessage(OperationType operation, int bytesCount) =>
        string.Format(MessageTemplate, GetMessageComponent(operation), bytesCount);
}
