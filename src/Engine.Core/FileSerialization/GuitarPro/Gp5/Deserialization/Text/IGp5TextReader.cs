using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Text;

internal interface IGp5TextReader
{
    ValueTask<Gp5ByteText> ReadByteTextAsync(int maxLength);
    ValueTask<string> ReadIntTextAsync();
    ValueTask<Gp5IntByteText> ReadIntByteTextAsync();
}
