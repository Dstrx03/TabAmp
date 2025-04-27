namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;

internal interface IFileDeserializationMetadata
{
    long? Length { get; }
    long? ProcessedBytes { get; }
}
