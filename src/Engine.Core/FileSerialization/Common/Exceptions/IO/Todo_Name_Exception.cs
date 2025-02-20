using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal sealed class Todo_Name_Exception : OperationException
{
    private new const string MessageTemplate = $"{OperationException.MessageTemplate}, the specified byte count must be a non-negative value.";

    public Todo_Name_Exception(OperationType operation, int count)
        : base(operation, count, ComposeMessage(operation, count))
    {
    }

    public Todo_Name_Exception(OperationType operation, int count, Exception inner)
        : base(operation, count, ComposeMessage(operation, count), inner)
    {
    }

    public static void ThrowIfNegative_TODO_NAME(OperationType operation, int count)
    {
        if (count < 0)
            throw new Todo_Name_Exception(operation, count);
    }

    private static string ComposeMessage(OperationType operation, int count) =>
        string.Format(MessageTemplate, operation, count);
}
