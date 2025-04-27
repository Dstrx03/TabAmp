namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;

internal interface IFileDeserializationMetadata
{
    long? Length { get; }
    long? ProcessedBytes { get; }

    float? ProcessedPercentage
    {
        get
        {
            if (Length is null || ProcessedBytes is null)
                return null;

            return ProcessedBytes * 100f / Length;
        }
    }
}
