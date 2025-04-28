using System;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;

internal interface IExactFileDeserializer : IFileDeserializationMetadataProvider
{
    void Todo_Name()
    {
        if (Metadata.Length is null || Metadata.ProcessedBytes is null)
            throw new Exception("TODO: metadata is null");

        ExactFileDeserializationException.ThrowIfNotExactlyDeserialized(Metadata);
    }

    internal sealed class ExactFileDeserializationException : FileSerializationException
    {
        private const string MessageTemplate = "The file was expected to be deserialized exactly";
        private const string Template = "Total length: {0} byte(s), processed: {1} byte(s).";

        private const string ZeroBytesProcessedMessageTemplate = $"{MessageTemplate}, but zero byte(s) were processed. {Template}";
        private const string FewerBytesProcessedMessageTemplate = $"{MessageTemplate}, but only {{1}} of {{0}} byte(s) were processed ({{2}}). {Template}";
        private const string MoreBytesProcessedMessageTemplate = $"{MessageTemplate}, but {{1}} byte(s) were processed instead of {{0}} byte(s) ({{2}}). {Template}";

        public ExactFileDeserializationException(IFileDeserializationMetadata metadata)
            : base(ComposeMessage(metadata))
        {
        }

        public static void ThrowIfNotExactlyDeserialized(IFileDeserializationMetadata metadata)
        {
            if (metadata.Length != metadata.ProcessedBytes)
                throw new ExactFileDeserializationException(metadata);
        }

        private static string ComposeMessage(IFileDeserializationMetadata metadata) =>
            string.Format(GetMessageTemplate(metadata),
                metadata.Length,
                metadata.ProcessedBytes,
                metadata.ProcessedPercentage);

        private static string GetMessageTemplate(IFileDeserializationMetadata metadata)
        {
            if (metadata.ProcessedBytes < metadata.Length)
                return FewerBytesProcessedMessageTemplate;
            else if (metadata.ProcessedBytes > metadata.Length)
                return MoreBytesProcessedMessageTemplate;
            else
                return ZeroBytesProcessedMessageTemplate;
        }
    }
}
