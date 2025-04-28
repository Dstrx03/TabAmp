namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;

internal interface IFileDeserializationMetadata
{
    long? Length { get; }
    long? ProcessedBytes { get; }

    float? ProcessedBytesRate
    {
        get
        {
            if (Length is null || ProcessedBytes is null)
                return null;

            return (float)((double)ProcessedBytes / (double)Length);
        }
    }
}
