using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;

internal interface IFileDeserializer<T> : IFileDeserializationMetadataProvider, IFileSerializationProcessor
{
    Task<T> DeserializeAsync();
}
