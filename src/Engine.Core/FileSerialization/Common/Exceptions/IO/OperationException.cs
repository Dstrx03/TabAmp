﻿using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal abstract class OperationException : FileSerializationException
{
    protected const string MessageTemplate = "Unable to {0} the next {1} byte(s)";

    protected OperationException(OperationType operation, int byteCount, string message)
        : base(message)
    {
        Operation = operation;
        ByteCount = byteCount;
    }

    protected OperationException(OperationType operation, int byteCount, string message, Exception inner)
        : base(message, inner)
    {
        Operation = operation;
        ByteCount = byteCount;
    }

    public OperationType Operation { get; }
    public int ByteCount { get; }

    protected static string GetOperationName(OperationType operation) => operation switch
    {
        OperationType.Read => "read",
        OperationType.Skip => "skip",
        _ => throw new ArgumentException()//TODO: exception?
    };
}
