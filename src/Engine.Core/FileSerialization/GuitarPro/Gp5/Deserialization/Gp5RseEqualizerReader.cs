using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

[Obsolete]
internal class Gp5RseEqualizerReader : IGp5RseEqualizerReader
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;

    public Gp5RseEqualizerReader(IGp5BinaryPrimitivesReader primitivesReader) =>
        _primitivesReader = primitivesReader;

    public async ValueTask<Gp5RseEqualizer> ReadRseEqualizerAsync(int bandsCount)
    {
        var bands = new sbyte[bandsCount];
        for (var i = 0; i < bands.Length; i++)
        {
            bands[i] = await _primitivesReader.ReadSignedByteAsync();
        }

        var gainPreFader = await _primitivesReader.ReadSignedByteAsync();

        return new Gp5RseEqualizer
        {
            Bands = bands,
            GainPreFader = gainPreFader
        };
    }
}
