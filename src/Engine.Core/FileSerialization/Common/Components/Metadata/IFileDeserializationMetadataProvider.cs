namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;

internal interface IFileDeserializationMetadataProvider
{
    IFileDeserializationMetadata Metadata { get; }
}
