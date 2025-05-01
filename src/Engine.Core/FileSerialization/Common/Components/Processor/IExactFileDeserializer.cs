using TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.Processor;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;

internal interface IExactFileDeserializer : IFileDeserializationMetadataProvider
{
    void ValidateExactDeserialization() => ExactFileDeserializationException.ThrowIfNotExactlyDeserialized(Metadata);
}
