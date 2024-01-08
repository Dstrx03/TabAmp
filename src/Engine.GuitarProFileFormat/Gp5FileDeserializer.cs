using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat.FileReader;
using TabAmp.Engine.GuitarProFileFormat.Models;

namespace TabAmp.Engine.GuitarProFileFormat;

public class Gp5FileDeserializer
{
    private readonly ISerialAsynchronousFileReader _fileReader;
    private readonly Gp5PrimitivesSerialDecoder _primitivesDecoder;
    private readonly Gp5CompositeTypesSerialDecoder _compositeTypesDecoder;

    private readonly Gp5File _file;

    public Gp5FileDeserializer(ISerialAsynchronousFileReader fileReader)
    {
        _fileReader = fileReader;
        _primitivesDecoder = new Gp5PrimitivesSerialDecoder(fileReader);
        _compositeTypesDecoder = new Gp5CompositeTypesSerialDecoder(fileReader, _primitivesDecoder);

        _file = new Gp5File();
    }

    public async Task<Gp5File> DeserializeAsync()
    {
        await ReadVersionAsync();
        await ReadScoreInformationAsync();
        await ReadLyricsAsync();
        await ReadRseMasterEffectAsync();
        await ReadPageSetupAsync();
        await ReadTempoAsync();
        return _file;
    }

    private async ValueTask ReadVersionAsync()
    {
        const int versionStringSize = 30;
        var versionString = await _compositeTypesDecoder.ReadStringOfByteLengthAsync(versionStringSize);

        _file.Version = versionString;

        // TODO:
        // "version" data is stored in size of 30 bytes, the actual version string is 24 characters long
        // remaining 6 bytes seems to have some arbitrary data - it may be not just trailing string bytes
        // does that 30 bytes is actually a "header" of guitar pro file?
    }

    private async ValueTask ReadScoreInformationAsync()
    {
        var scoreInformation = new Gp5ScoreInformation
        {
            Title = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Subtitle = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Artist = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Album = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Words = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Music = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Copyright = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Tab = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Instructions = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Notice = new string[await _primitivesDecoder.ReadIntAsync()]
        };

        for (var i = 0; i < scoreInformation.Notice.Length; i++)
        {
            var noticeLine = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync();
            scoreInformation.Notice[i] = noticeLine;
        }

        _file.ScoreInformation = scoreInformation;
    }

    private async ValueTask ReadLyricsAsync()
    {
        const int lyricsLinesCount = 5;
        var lyrics = new Gp5Lyrics
        {
            ApplyToTrack = await _primitivesDecoder.ReadIntAsync(),
            Lines = new Gp5LyricsLine[lyricsLinesCount]
        };

        for (var i = 0; i < lyrics.Lines.Length; i++)
        {
            lyrics.Lines[i] = new Gp5LyricsLine
            {
                StartFromBar = await _primitivesDecoder.ReadIntAsync(),
                Lyrics = await _compositeTypesDecoder.ReadStringOfIntLengthAsync()
            };
        }

        _file.Lyrics = lyrics;
    }

    private async ValueTask ReadRseMasterEffectAsync()
    {
        const int rseMasterEffectEqualizerBandsCount = 10;
        var masterEffect = new Gp5RseMasterEffect
        {
            Volume = await _primitivesDecoder.ReadIntAsync(),
            _A01 = await _primitivesDecoder.ReadIntAsync(),
            Equalizer = await _compositeTypesDecoder.ReadRseEqualizerAsync(rseMasterEffectEqualizerBandsCount)
        };

        _file.RseMasterEffect = masterEffect;
    }

    private async ValueTask ReadPageSetupAsync()
    {
        var pageSetup = new Gp5PageSetup
        {
            Width = await _primitivesDecoder.ReadIntAsync(),
            Height = await _primitivesDecoder.ReadIntAsync(),
            MarginLeft = await _primitivesDecoder.ReadIntAsync(),
            MarginRight = await _primitivesDecoder.ReadIntAsync(),
            MarginTop = await _primitivesDecoder.ReadIntAsync(),
            MarginBottom = await _primitivesDecoder.ReadIntAsync(),
            ScoreSizeProportion = await _primitivesDecoder.ReadIntAsync(),
            HeaderAndFooter = (Gp5PageSetup.HeaderAndFooterFlags)await _primitivesDecoder.ReadShortAsync(),
            Title = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Subtitle = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Artist = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Album = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Words = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Music = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            WordsAndMusic = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            CopyrightFirstLine = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            CopyrightSecondLine = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            PageNumber = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync()
        };

        _file.PageSetup = pageSetup;
    }

    private async ValueTask ReadTempoAsync()
    {
        var tempo = new Gp5Tempo
        {
            WordIndication = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            BeatsPerMinute = await _primitivesDecoder.ReadIntAsync(),
            HideBeatsPerMinute = await _primitivesDecoder.ReadBoolAsync()
        };

        _file.Tempo = tempo;
    }
}
