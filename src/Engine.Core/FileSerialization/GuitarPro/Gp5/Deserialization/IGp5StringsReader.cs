using System.Threading.Tasks;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal interface IGp5StringsReader
{
    ValueTask<string> ReadByteStringAsync(int maxLength);
    ValueTask<string> ReadIntStringAsync();
    ValueTask<string> ReadIntByteStringAsync();
}
