using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5ColorReader : IGp5ColorReader
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;

    public Gp5ColorReader(IGp5BinaryPrimitivesReader primitivesReader) =>
        _primitivesReader = primitivesReader;

    public async ValueTask<Gp5Color> ReadColorAsync()
    {
        return new Gp5Color
        {
            Red = await _primitivesReader.ReadByteAsync(),
            Green = await _primitivesReader.ReadByteAsync(),
            Blue = await _primitivesReader.ReadByteAsync(),
            Alpha = await _primitivesReader.ReadByteAsync()
        };
    }
}
