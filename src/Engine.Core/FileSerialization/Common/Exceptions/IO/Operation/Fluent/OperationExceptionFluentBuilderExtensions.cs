using System;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation;

internal static class OperationExceptionFluentBuilderExtensions
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


    public static NegativeBytesCountOperationException Build(
        this IOperationExceptionFluentBuilder<NegativeBytesCountOperationException> builder,
        int bytesCount) => new(builder.Operation, bytesCount);

    public static NegativeBytesCountOperationException Build(
        this IOperationExceptionFluentBuilder<NegativeBytesCountOperationException> builder,
        int bytesCount,
        Exception inner) => new(builder.Operation, bytesCount, inner);

    public static void ThrowIfNegative(
        this IOperationExceptionFluentBuilder<NegativeBytesCountOperationException> builder,
        int bytesCount)
    {
        if (bytesCount < 0)
            throw builder.Build(bytesCount);
    }
}
