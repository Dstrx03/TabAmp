using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Fluent;

internal static class EndOfFileOperationExceptionFluentBuilderExtensions
{
    public static EndOfFileOperationException Build(
        this IOperationExceptionFluentBuilder<EndOfFileOperationException> builder,
        int bytesCount,
        int trailingBytesCount) => new(builder.Operation, bytesCount, trailingBytesCount);

    public static EndOfFileOperationException Build(
        this IOperationExceptionFluentBuilder<EndOfFileOperationException> builder,
        int bytesCount,
        int trailingBytesCount,
        Exception inner) => new(builder.Operation, bytesCount, trailingBytesCount, inner);

    public static void ThrowIfTrailing(
        this IOperationExceptionFluentBuilder<EndOfFileOperationException> builder,
        int bytesCount,
        long trailingBytesCount)
    {
        if (trailingBytesCount > 0)
            throw builder.Build(bytesCount, (int)trailingBytesCount);
    }
}
