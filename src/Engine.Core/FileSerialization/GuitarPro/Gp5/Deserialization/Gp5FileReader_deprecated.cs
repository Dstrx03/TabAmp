using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5FileReader_deprecated
{
    private readonly Gp5TypesReader_deprecated _reader;

    public Gp5FileReader_deprecated(Gp5TypesReader_deprecated reader) =>
        _reader = reader;

    public async ValueTask<Gp5LyricsLine> ReadLyricsLineAsync()
    {
        return new Gp5LyricsLine
        {
            StartFromBar = await _reader.ReadIntAsync(),
            Lyrics = await _reader.ReadIntStringAsync()
        };
    }

    public async ValueTask<Gp5RseEqualizer> ReadRseEqualizerAsync(int bandsCount)
    {
        var bands = new sbyte[bandsCount];
        for (var i = 0; i < bands.Length; i++)
        {
            bands[i] = await _reader.ReadSignedByteAsync();
        }

        var gainPreFader = await _reader.ReadSignedByteAsync();

        return new Gp5RseEqualizer
        {
            Bands = bands,
            GainPreFader = gainPreFader
        };
    }

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
