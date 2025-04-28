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
            // TODO: overflow is expected, resolve
            return (float)ProcessedBytes / Length;
        }
    }
}
