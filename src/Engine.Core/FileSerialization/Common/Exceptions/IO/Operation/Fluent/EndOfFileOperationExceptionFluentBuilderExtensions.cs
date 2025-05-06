using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

internal static class EndOfFileOperationExceptionFluentBuilderExtensions
{
    public static EndOfFileOperationException Build(
        this IOperationExceptionFluentBuilder<EndOfFileOperationException> builder,
        int bytesCount,
        long trailingBytesCount) => new(builder.Operation, bytesCount, trailingBytesCount);

    public static EndOfFileOperationException Build(
        this IOperationExceptionFluentBuilder<EndOfFileOperationException> builder,
        int bytesCount,
        long trailingBytesCount,
        Exception inner) => new(builder.Operation, bytesCount, trailingBytesCount, inner);

    public static void ThrowIfTrailing(
        this IOperationExceptionFluentBuilder<EndOfFileOperationException> builder,
        int bytesCount,
        long trailingBytesCount)
    {
        if (trailingBytesCount > 0)
            throw builder.Build(bytesCount, trailingBytesCount);
    }
}
