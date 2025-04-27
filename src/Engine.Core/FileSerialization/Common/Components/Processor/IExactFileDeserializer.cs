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
        private const string MessageTemplate = "The file was expected to be deserialized exactly, but {0}. Total length: {1} byte(s), processed: {2} byte(s).";

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
            string.Format(MessageTemplate, 
                GetMessageComponent(metadata), 
                metadata.Length, 
                metadata.ProcessedBytes, 
                metadata.ProcessedPercentage);

        private static string GetMessageComponent(IFileDeserializationMetadata metadata)
        {
            if (metadata.ProcessedBytes < metadata.Length)
                return "only {2} of {1} byte(s) were processed ({3}%)";
            else if (metadata.ProcessedBytes > metadata.Length)
                return "{2} byte(s) were processed instead of {1} byte(s) ({3}%)";
            else
                return "zero bytes were processed";
        }
    }
}
