using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5FileReader_deprecated
{
    private readonly Gp5TypesReader_deprecated _reader;

    public Gp5FileReader_deprecated(Gp5TypesReader_deprecated reader) =>
        _reader = reader;

    public async ValueTask<Gp5Color> ReadColorAsync()
    {
        return new Gp5Color
        {
            Red = await _reader.ReadByteAsync(),
            Green = await _reader.ReadByteAsync(),
            Blue = await _reader.ReadByteAsync(),
            Alpha = await _reader.ReadByteAsync()
        };
    }
}
