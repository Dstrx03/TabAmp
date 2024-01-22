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
        await ReadHeaderTempoAsync();
        await ReadHeaderKeySignatureAsync();
        await ReadMidiChannelsAsync();
        await ReadMusicalDirectionsAsync();
        await ReadRseMasterEffectReverbAsync();
        await ReadMeasuresCountAsync();
        await ReadTracksCountAsync();
        await ReadMeasureHeadersAsync();
        return _file;
    }

    private async ValueTask ReadVersionAsync()
    {
        const int versionStringMaxLength = 30;
        var versionString = await _compositeTypesDecoder.ReadByteStringAsync(versionStringMaxLength);

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
            Title = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Subtitle = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Artist = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Album = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Words = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Music = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Copyright = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Tab = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Instructions = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Notice = new string[await _primitivesDecoder.ReadIntAsync()]
        };

        for (var i = 0; i < scoreInformation.Notice.Length; i++)
        {
            var noticeLine = await _compositeTypesDecoder.ReadIntByteStringAsync();
            scoreInformation.Notice[i] = noticeLine;
        }

        _file.ScoreInformation = scoreInformation;
    }

    private async ValueTask ReadLyricsAsync()
    {
        var lyrics = new Gp5Lyrics
        {
            ApplyToTrack = await _primitivesDecoder.ReadIntAsync(),
            FirstLine = await _compositeTypesDecoder.ReadLyricsLineAsync(),
            SecondLine = await _compositeTypesDecoder.ReadLyricsLineAsync(),
            ThirdLine = await _compositeTypesDecoder.ReadLyricsLineAsync(),
            FourthLine = await _compositeTypesDecoder.ReadLyricsLineAsync(),
            FifthLine = await _compositeTypesDecoder.ReadLyricsLineAsync()
        };

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
            HeaderAndFooterFlags = (Gp5PageSetup.HeaderAndFooter)await _primitivesDecoder.ReadShortAsync(),
            Title = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Subtitle = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Artist = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Album = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Words = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            Music = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            WordsAndMusic = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            CopyrightFirstLine = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            CopyrightSecondLine = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            PageNumber = await _compositeTypesDecoder.ReadIntByteStringAsync()
        };

        _file.PageSetup = pageSetup;
    }

    private async ValueTask ReadHeaderTempoAsync()
    {
        var tempo = new Gp5Tempo
        {
            WordIndication = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            BeatsPerMinute = await _primitivesDecoder.ReadIntAsync(),
            HideBeatsPerMinute = await _primitivesDecoder.ReadBoolAsync()
        };

        _file.Tempo = tempo;
    }

    private async ValueTask ReadHeaderKeySignatureAsync()
    {
        var keySignature = new Gp5HeaderKeySignature
        {
            Key = await _primitivesDecoder.ReadSignedByteAsync(),
            _A01 = await _primitivesDecoder.ReadSignedByteAsync(),
            _A02 = await _primitivesDecoder.ReadSignedByteAsync(),
            _A03 = await _primitivesDecoder.ReadSignedByteAsync(),
            Octave = await _primitivesDecoder.ReadSignedByteAsync()
        };

        _file.KeySignature = keySignature;
    }

    private async ValueTask ReadMidiChannelsAsync()
    {
        const int midiChannelsCount = 64;
        var midiChannels = new Gp5MidiChannel[midiChannelsCount];
        for (var i = 0; i < midiChannels.Length; i++)
        {
            midiChannels[i] = new Gp5MidiChannel
            {
                Instrument = await _primitivesDecoder.ReadIntAsync(),
                Volume = await _primitivesDecoder.ReadByteAsync(),
                Balance = await _primitivesDecoder.ReadByteAsync(),
                Chorus = await _primitivesDecoder.ReadByteAsync(),
                Reverb = await _primitivesDecoder.ReadByteAsync(),
                Phaser = await _primitivesDecoder.ReadByteAsync(),
                Tremolo = await _primitivesDecoder.ReadByteAsync(),
                _A01 = await _primitivesDecoder.ReadByteAsync(),
                _A02 = await _primitivesDecoder.ReadByteAsync()
            };
        }

        _file.MidiChannels = midiChannels;
    }

    private async ValueTask ReadMusicalDirectionsAsync()
    {
        var musicalDirections = new Gp5MusicalDirections
        {
            Coda = await _primitivesDecoder.ReadShortAsync(),
            DoubleCoda = await _primitivesDecoder.ReadShortAsync(),
            Segno = await _primitivesDecoder.ReadShortAsync(),
            SegnoSegno = await _primitivesDecoder.ReadShortAsync(),
            Fine = await _primitivesDecoder.ReadShortAsync(),
            DaCapo = await _primitivesDecoder.ReadShortAsync(),
            DaCapoAlCoda = await _primitivesDecoder.ReadShortAsync(),
            DaCapoAlDoubleCoda = await _primitivesDecoder.ReadShortAsync(),
            DaCapoAlFine = await _primitivesDecoder.ReadShortAsync(),
            DaSegno = await _primitivesDecoder.ReadShortAsync(),
            DaSegnoAlCoda = await _primitivesDecoder.ReadShortAsync(),
            DaSegnoAlDoubleCoda = await _primitivesDecoder.ReadShortAsync(),
            DaSegnoAlFine = await _primitivesDecoder.ReadShortAsync(),
            DaSegnoSegno = await _primitivesDecoder.ReadShortAsync(),
            DaSegnoSegnoAlCoda = await _primitivesDecoder.ReadShortAsync(),
            DaSegnoSegnoAlDoubleCoda = await _primitivesDecoder.ReadShortAsync(),
            DaSegnoSegnoAlFine = await _primitivesDecoder.ReadShortAsync(),
            DaCoda = await _primitivesDecoder.ReadShortAsync(),
            DaDoubleCoda = await _primitivesDecoder.ReadShortAsync()
        };

        _file.MusicalDirections = musicalDirections;
    }

    private async ValueTask ReadRseMasterEffectReverbAsync()
    {
        var reverb = await _primitivesDecoder.ReadIntAsync();

        _file.RseMasterEffect.Reverb = reverb;
    }

    private async ValueTask ReadMeasuresCountAsync()
    {
        var measuresCount = await _primitivesDecoder.ReadIntAsync();

        _file.MeasuresCount = measuresCount;
    }

    private async ValueTask ReadTracksCountAsync()
    {
        var tracksCount = await _primitivesDecoder.ReadIntAsync();

        _file.TracksCount = tracksCount;
    }

    private async ValueTask ReadMeasureHeadersAsync()
    {
        var measureHeaders = new Gp5MeasureHeader[_file.MeasuresCount];
        for (var i = 0; i < measureHeaders.Length; i++)
        {
            measureHeaders[i] = await _compositeTypesDecoder.ReadMeasureHeaderAsync(isFirst: i == 0);
        }

        _file.MeasureHeaders = measureHeaders;
    }
}
