using System.Threading.Tasks;
using TabAmp.Engine.Core.Score;

namespace TabAmp.Engine.Core.FileSerialization;

internal interface IFileSerializationProcessor
{
    Task ProcessAsync();
}

internal interface IFileDeserializer : IFileSerializationProcessor
{
}
