using System.Threading.Tasks;

namespace TabAmp.Engine.Core.FileSerialization.Common.Processor;

internal interface IFileDeserializer<T> : IFileSerializationProcessor
{
    Task<T> DeserializeAsync();
}
