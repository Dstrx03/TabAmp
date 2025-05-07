using System;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation;

internal static class NegativeBytesCountOperationExceptionFluentBuilderExtensions
{
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
