using System.Threading.Tasks;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;

internal interface IFileDeserializer<T> : IFileSerializationProcessor
{
    Task<T> DeserializeAsync();
}
