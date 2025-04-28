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
        private const string MainMessageToken = "The file was expected to be deserialized exactly";
        private const string InfoMessageToken = "Total length: {0} byte(s), processed: {1} byte(s).";

        private const string MessageTemplate = $"{MainMessageToken}. {InfoMessageToken}";
        private const string ZeroBytesProcessedMessageTemplate = $"{MainMessageToken}, but zero byte(s) were processed. {InfoMessageToken}";
        private const string FewerBytesProcessedMessageTemplate = $"{MainMessageToken}, but only {{1}} of {{0}} byte(s) were processed ({{2}}). {InfoMessageToken}";
        private const string MoreBytesProcessedMessageTemplate = $"{MainMessageToken}, but {{1}} byte(s) were processed instead of {{0}} byte(s) ({{2}}). {InfoMessageToken}";

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
                $"{metadata.ProcessedBytesRate:P2}");

        private static string GetMessageTemplate(IFileDeserializationMetadata metadata)
        {
            if (metadata.ProcessedBytes <= 0)
                return ZeroBytesProcessedMessageTemplate;

            if (metadata.ProcessedBytes < metadata.Length)
                return FewerBytesProcessedMessageTemplate;
            else if (metadata.ProcessedBytes > metadata.Length)
                return MoreBytesProcessedMessageTemplate;

            return MessageTemplate;
        }
    }
}
