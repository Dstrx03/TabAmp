using System;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.Processor;

internal sealed class ExactFileDeserializationException : FileSerializationException
{
    private const string MessageToken = "The file must be deserialized exactly";
    private const string PercentageDeviationMessageToken = "({2}, deviation: {3})";
    private const string BoundaryMetadataMessageToken = "Total length: {0} byte(s), processed: {1} byte(s).";

    private const string BoundaryMetadataMissingMessageTemplate = $"{MessageToken}, but required boundary metadata is missing. {BoundaryMetadataMessageToken}";
    private const string ZeroBytesProcessedMessageTemplate = $"{MessageToken}, but zero byte(s) were processed. {BoundaryMetadataMessageToken}";
    private const string FewerBytesProcessedMessageTemplate = $"{MessageToken}, but only {{1}} of {{0}} byte(s) were processed {PercentageDeviationMessageToken}. {BoundaryMetadataMessageToken}";
    private const string MoreBytesProcessedMessageTemplate = $"{MessageToken}, but {{1}} byte(s) were processed instead of {{0}} byte(s) {PercentageDeviationMessageToken}. {BoundaryMetadataMessageToken}";

    public ExactFileDeserializationException(string message)
        : base(message)
    {
    }

    public ExactFileDeserializationException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public static void ThrowIfNotExactlyDeserialized(IFileDeserializationMetadata metadata)
    {
        if (IsBoundaryMetadataMissing(metadata) || IsNotExactlyDeserialized(metadata))
            throw new ExactFileDeserializationException(ComposeMessage(metadata));
    }

    private static bool IsBoundaryMetadataMissing(IFileDeserializationMetadata metadata) =>
        metadata.Length is null || metadata.ProcessedBytes is null;

    private static bool IsNotExactlyDeserialized(IFileDeserializationMetadata metadata) =>
        metadata.Length != metadata.ProcessedBytes;

    private static string ComposeMessage(IFileDeserializationMetadata metadata) =>
        string.Format(GetMessageTemplate(metadata),
            FormatNullableMessageComponent(metadata.Length),
            FormatNullableMessageComponent(metadata.ProcessedBytes),
            GetPercentageMessageComponent(metadata),
            GetDeviationMessageComponent(metadata));

    private static string GetMessageTemplate(IFileDeserializationMetadata metadata)
    {
        if (IsBoundaryMetadataMissing(metadata))
            return BoundaryMetadataMissingMessageTemplate;

        if (metadata.ProcessedBytes == 0)
            return ZeroBytesProcessedMessageTemplate;

        if (metadata.ProcessedBytes < metadata.Length)
            return FewerBytesProcessedMessageTemplate;

        return MoreBytesProcessedMessageTemplate;
    }

    private static string GetPercentageMessageComponent(IFileDeserializationMetadata metadata) =>
        metadata.ProcessedBytesRate is not null ? $"{metadata.ProcessedBytesRate:P2}" : string.Empty;

    private static string GetDeviationMessageComponent(IFileDeserializationMetadata metadata)
    {
        if (metadata.Length is not long length || metadata.ProcessedBytes is not long processed)
            return string.Empty;

        var deviation = processed - length;
        var signToken = deviation > 0 ? "+" : "";

        return $"{signToken}{deviation} B";
    }

    private static string FormatNullableMessageComponent<T>(T? value)
        where T : struct => value?.ToString() ?? "'null'";
}
