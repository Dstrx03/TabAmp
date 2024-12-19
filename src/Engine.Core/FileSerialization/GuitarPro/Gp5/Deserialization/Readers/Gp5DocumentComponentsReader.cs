using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.DocumentComponents;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Readers;

internal class Gp5DocumentComponentsReader : IGp5DocumentComponentsReader
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;
    private readonly IGp5TextReader _textReader;

    public Gp5DocumentComponentsReader(IGp5BinaryPrimitivesReader primitivesReader, IGp5TextReader textReader) =>
        (_primitivesReader, _textReader) = (primitivesReader, textReader);

    public ValueTask<Gp5ByteText> ReadVersionAsync() =>
        _textReader.ReadByteTextAsync(Gp5File.VersionMaxLength);

    public async ValueTask<(int measureHeadersCount, int tracksCount)> ReadMeasureHeadersAndTracksCountAsync() =>
        (measureHeadersCount: await _primitivesReader.ReadIntAsync(), tracksCount: await _primitivesReader.ReadIntAsync());

    public async ValueTask<Gp5ScoreInformation> ReadScoreInformationAsync()
    {
        var scoreInformation = new Gp5ScoreInformation
        {
            Title = await _textReader.ReadIntByteTextAsync(),
            Subtitle = await _textReader.ReadIntByteTextAsync(),
            Artist = await _textReader.ReadIntByteTextAsync(),
            Album = await _textReader.ReadIntByteTextAsync(),
            Words = await _textReader.ReadIntByteTextAsync(),
            Music = await _textReader.ReadIntByteTextAsync(),
            Copyright = await _textReader.ReadIntByteTextAsync(),
            Tab = await _textReader.ReadIntByteTextAsync(),
            Instructions = await _textReader.ReadIntByteTextAsync(),
            Notice = new string[await _primitivesReader.ReadIntAsync()]
        };

        for (var i = 0; i < scoreInformation.Notice.Length; i++)
        {
            scoreInformation.Notice[i] = await _textReader.ReadIntByteTextAsync();
        }

        return scoreInformation;
    }

    public async ValueTask<Gp5PageSetup> ReadPageSetupAsync()
    {
        return new Gp5PageSetup
        {
            Width = await _primitivesReader.ReadIntAsync(),
            Height = await _primitivesReader.ReadIntAsync(),
            MarginLeft = await _primitivesReader.ReadIntAsync(),
            MarginRight = await _primitivesReader.ReadIntAsync(),
            MarginTop = await _primitivesReader.ReadIntAsync(),
            MarginBottom = await _primitivesReader.ReadIntAsync(),
            ScoreSizeProportion = await _primitivesReader.ReadIntAsync(),
            HeaderAndFooterFlags = (Gp5PageSetup.HeaderAndFooter)await _primitivesReader.ReadShortAsync(),
            Title = await _textReader.ReadIntByteTextAsync(),
            Subtitle = await _textReader.ReadIntByteTextAsync(),
            Artist = await _textReader.ReadIntByteTextAsync(),
            Album = await _textReader.ReadIntByteTextAsync(),
            Words = await _textReader.ReadIntByteTextAsync(),
            Music = await _textReader.ReadIntByteTextAsync(),
            WordsAndMusic = await _textReader.ReadIntByteTextAsync(),
            CopyrightFirstLine = await _textReader.ReadIntByteTextAsync(),
            CopyrightSecondLine = await _textReader.ReadIntByteTextAsync(),
            PageNumber = await _textReader.ReadIntByteTextAsync()
        };
    }

    public async ValueTask<Gp5Lyrics> ReadLyricsAsync()
    {
        var lyrics = new Gp5Lyrics
        {
            ApplyToTrack = await _primitivesReader.ReadIntAsync(),
            Lines = new (int, string)[Gp5Lyrics.LinesLength]
        };

        for (var i = 0; i < lyrics.Lines.Length; i++)
        {
            var startFromBar = await _primitivesReader.ReadIntAsync();
            var text = await _textReader.ReadIntTextAsync();

            lyrics.Lines[i] = (startFromBar, text);
        }

        return lyrics;
    }

    public async ValueTask<Gp5Marker> ReadMarkerAsync()
    {
        return new Gp5Marker
        {
            Name = await _textReader.ReadIntByteTextAsync(),
            Color = await _primitivesReader.ReadColorAsync()
        };
    }
}
