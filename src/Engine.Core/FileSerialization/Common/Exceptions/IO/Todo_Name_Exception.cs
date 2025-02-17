using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;

internal sealed class Todo_Name_Exception : OperationException
{
    public Todo_Name_Exception(OperationType operation, int count)
        : base(operation, count, Format(operation, count))
    {
    }

    public Todo_Name_Exception(OperationType operation, int count, Exception inner)
        : base(operation, count, Format(operation, count), inner)
    {
    }

    private static string Format(OperationType operation, int count) =>
        $"Unable to {operation} the next {count} byte(s), the specified byte count must be a non-negative value.";
}
