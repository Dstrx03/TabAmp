using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Readers;

internal class Gp5RseReader : IGp5RseReader
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;
    private readonly IGp5TextReader _textReader;

    public Gp5RseReader(IGp5BinaryPrimitivesReader primitivesReader, IGp5TextReader textReader) =>
        (_primitivesReader, _textReader) = (primitivesReader, textReader);

    public async ValueTask<Gp5RseInstrument> ReadRseInstrumentAsync()
    {
        return new Gp5RseInstrument
        {
            Instrument = await _primitivesReader.ReadIntAsync(),
            _A01 = await _primitivesReader.ReadIntAsync(),
            SoundBank = await _primitivesReader.ReadIntAsync(),
            _B01 = await _primitivesReader.ReadIntAsync()
        };
    }

    public async ValueTask<Gp5RseInstrumentEffect> ReadRseInstrumentEffectAsync()
    {
        return new Gp5RseInstrumentEffect
        {
            Name = await _textReader.ReadIntByteTextAsync(),
            CategoryName = await _textReader.ReadIntByteTextAsync()
        };
    }

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
