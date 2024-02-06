using System;
using System.Text.Json;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.Score;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal sealed class Gp5FileDeserializer : Gp5FileSerializationProcessor, IFileDeserializer<Gp5Score>
{
    private readonly Gp5PrimitivesSerialDecoder _primitivesDecoder;
    private readonly Gp5CompositeTypesSerialDecoder _compositeTypesDecoder;

    public Gp5FileDeserializer(Gp5PrimitivesSerialDecoder primitivesDecoder, Gp5CompositeTypesSerialDecoder compositeTypesDecoder)
    {
        _primitivesDecoder = primitivesDecoder;
        _compositeTypesDecoder = compositeTypesDecoder;
    }

    public async Task<Gp5Score> DeserializeAsync()
    {
        _file = new Gp5File();
        await ProcessAsync();

        Console.WriteLine(JsonSerializer.Serialize(_file, new JsonSerializerOptions { WriteIndented = true }));

        return new Gp5Score();
    }

    protected override async ValueTask NextVersionAsync() =>
        _file.Version = await ReadVersionAsync();

    protected override async ValueTask NextScoreInformationAsync() =>
        _file.ScoreInformation = await ReadScoreInformationAsync();

    protected override async ValueTask NextLyricsAsync() =>
        _file.Lyrics = await ReadLyricsAsync();

    protected override async ValueTask NextRseMasterEffectAsync() =>
        _file.RseMasterEffect = await ReadRseMasterEffectAsync();

    protected override async ValueTask NextPageSetupAsync() =>
        _file.PageSetup = await ReadPageSetupAsync();

    protected override async ValueTask NextHeaderTempoAsync() =>
        _file.Tempo = await ReadHeaderTempoAsync();

    protected override async ValueTask NextHeaderKeySignatureAsync() =>
        _file.KeySignature = await ReadHeaderKeySignatureAsync();

    protected override async ValueTask NextMidiChannelsAsync() =>
        _file.MidiChannels = await ReadMidiChannelsAsync();

    protected override async ValueTask NextMusicalDirectionsAsync() =>
        _file.MusicalDirections = await ReadMusicalDirectionsAsync();

    protected override async ValueTask NextRseMasterEffectReverbAsync() =>
        _file.RseMasterEffect.Reverb = await _primitivesDecoder.ReadIntAsync();

    protected override async ValueTask NextMeasuresCountAsync() =>
        _file.MeasuresCount = await _primitivesDecoder.ReadIntAsync();

    protected override async ValueTask NextTracksCountAsync() =>
        _file.TracksCount = await _primitivesDecoder.ReadIntAsync();

    protected override async ValueTask NextMeasureHeadersAsync() =>
        _file.MeasureHeaders = await ReadMeasureHeadersAsync();


    private async ValueTask<string> ReadVersionAsync()
    {
        const int versionStringMaxLength = 30;
        var versionString = await _compositeTypesDecoder.ReadByteStringAsync(versionStringMaxLength);

        return versionString;

        // TODO:
        // "version" data is stored in size of 30 bytes, the actual version string is 24 characters long
        // remaining 6 bytes seems to have some arbitrary data - it may be not just trailing string bytes
        // does that 30 bytes is actually a "header" of guitar pro file?
    }

    private async ValueTask<Gp5ScoreInformation> ReadScoreInformationAsync()
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

        return scoreInformation;
    }

    private async ValueTask<Gp5Lyrics> ReadLyricsAsync()
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

        return lyrics;
    }

    private async ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync()
    {
        const int rseMasterEffectEqualizerBandsCount = 10;
        var masterEffect = new Gp5RseMasterEffect
        {
            Volume = await _primitivesDecoder.ReadIntAsync(),
            _A01 = await _primitivesDecoder.ReadIntAsync(),
            Equalizer = await _compositeTypesDecoder.ReadRseEqualizerAsync(rseMasterEffectEqualizerBandsCount)
        };

        return masterEffect;
    }

    private async ValueTask<Gp5PageSetup> ReadPageSetupAsync()
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

        return pageSetup;
    }

    private async ValueTask<Gp5Tempo> ReadHeaderTempoAsync()
    {
        var tempo = new Gp5Tempo
        {
            WordIndication = await _compositeTypesDecoder.ReadIntByteStringAsync(),
            BeatsPerMinute = await _primitivesDecoder.ReadIntAsync(),
            HideBeatsPerMinute = await _primitivesDecoder.ReadBoolAsync()
        };

        return tempo;
    }

    private async ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync()
    {
        var keySignature = new Gp5HeaderKeySignature
        {
            Key = await _primitivesDecoder.ReadSignedByteAsync(),
            _A01 = await _primitivesDecoder.ReadSignedByteAsync(),
            _A02 = await _primitivesDecoder.ReadSignedByteAsync(),
            _A03 = await _primitivesDecoder.ReadSignedByteAsync(),
            Octave = await _primitivesDecoder.ReadSignedByteAsync()
        };

        return keySignature;
    }

    private async ValueTask<Gp5MidiChannel[]> ReadMidiChannelsAsync()
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

        return midiChannels;
    }

    private async ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync()
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

        return musicalDirections;
    }

    private async ValueTask<Gp5MeasureHeader[]> ReadMeasureHeadersAsync()
    {
        var measureHeaders = new Gp5MeasureHeader[_file.MeasuresCount];
        for (var i = 0; i < measureHeaders.Length; i++)
        {
            measureHeaders[i] = await _compositeTypesDecoder.ReadMeasureHeaderAsync(isFirst: i == 0);
        }

        return measureHeaders;
    }
}
