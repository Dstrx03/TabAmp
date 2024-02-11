using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5CompositeTypesSerialDecoder
{
    private readonly Gp5GeneralTypesDeserializer _deserializer;

    public Gp5CompositeTypesSerialDecoder(Gp5GeneralTypesDeserializer deserializer) =>
        _deserializer = deserializer;

    // TODO: refactoring

    public async ValueTask<Gp5RseEqualizer> ReadRseEqualizerAsync(int bandsCount)
    {
        var bands = new sbyte[bandsCount];
        for (var i = 0; i < bands.Length; i++)
        {
            bands[i] = await _deserializer.ReadSignedByteAsync();
        }

        var gainPreFader = await _deserializer.ReadSignedByteAsync();

        return new Gp5RseEqualizer
        {
            Bands = bands,
            GainPreFader = gainPreFader
        };
    }

    public async ValueTask<Gp5TimeSignature> ReadTimeSignatureAsync(bool hasNumerator, bool hasDenominator)
    {
        if (!hasNumerator && !hasDenominator)
            return null;

        return new Gp5TimeSignature
        {
            Numerator = hasNumerator ? await _deserializer.ReadByteAsync() : null,
            Denominator = hasDenominator ? await _deserializer.ReadByteAsync() : null
        };
    }

    public async ValueTask<Gp5TimeSignatureBeamGroups> ReadTimeSignatureBeamGroupsAsync()
    {
        return new Gp5TimeSignatureBeamGroups
        {
            FirstSpan = await _deserializer.ReadByteAsync(),
            SecondSpan = await _deserializer.ReadByteAsync(),
            ThirdSpan = await _deserializer.ReadByteAsync(),
            FourthSpan = await _deserializer.ReadByteAsync()
        };
    }

    public async ValueTask<Gp5Marker> ReadMarkerAsync()
    {
        return new Gp5Marker
        {
            Name = await _deserializer.ReadIntByteStringAsync(),
            Color = await ReadColorAsync()
        };
    }

    public async ValueTask<Gp5Color> ReadColorAsync()
    {
        return new Gp5Color
        {
            Red = await _deserializer.ReadByteAsync(),
            Green = await _deserializer.ReadByteAsync(),
            Blue = await _deserializer.ReadByteAsync(),
            Alpha = await _deserializer.ReadByteAsync()
        };
    }

    public async ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst)
    {
        var primaryFlags = (Gp5MeasureHeader.Primary)await _deserializer.ReadByteAsync();
        var measureHeader = new Gp5MeasureHeader
        {
            PrimaryFlags = primaryFlags
        };

        var hasNumerator = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignatureNumerator);
        var hasDenominator = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignatureDenominator);
        var hasBeamGroups = hasNumerator || hasDenominator;
        measureHeader.TimeSignature = await ReadTimeSignatureAsync(hasNumerator: hasNumerator, hasDenominator: hasDenominator);

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasRepeatClose))
            measureHeader.RepeatCount = await _deserializer.ReadByteAsync();

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasMarker))
            measureHeader.Marker = await ReadMarkerAsync();

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasKeySignature))
            measureHeader.KeySignature = await ReadKeySignatureAsync();

        var hasAlternateEndings = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasAlternateEndings);
        if (isFirst)
        {
            if (hasBeamGroups)
                measureHeader.TimeSignature.BeamGroups = await ReadTimeSignatureBeamGroupsAsync();

            if (hasAlternateEndings)
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _deserializer.ReadByteAsync();
        }
        else
        {
            if (hasAlternateEndings)
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _deserializer.ReadByteAsync();

            if (hasBeamGroups)
                measureHeader.TimeSignature.BeamGroups = await ReadTimeSignatureBeamGroupsAsync();
        }

        if (!hasAlternateEndings)
            measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _deserializer.ReadByteAsync();

        measureHeader.TripletFeel = await _deserializer.ReadByteAsync();
        measureHeader.EndOfObjectSeparator = await _deserializer.ReadByteAsync();

        return measureHeader;
    }

    public async ValueTask<Gp5KeySignature> ReadKeySignatureAsync()
    {
        return new Gp5KeySignature
        {
            Key = await _deserializer.ReadSignedByteAsync(),
            IsMinorKey = await _deserializer.ReadBoolAsync()
        };
    }

    public async ValueTask<Gp5LyricsLine> ReadLyricsLineAsync()
    {
        return new Gp5LyricsLine
        {
            StartFromBar = await _deserializer.ReadIntAsync(),
            Lyrics = await _deserializer.ReadIntStringAsync()
        };
    }

    public async ValueTask<string> ReadVersionAsync()
    {
        const int versionStringMaxLength = 30;
        var versionString = await _deserializer.ReadByteStringAsync(versionStringMaxLength);

        return versionString;

        // TODO:
        // "version" data is stored in size of 30 bytes, the actual version string is 24 characters long
        // remaining 6 bytes seems to have some arbitrary data - it may be not just trailing string bytes
        // does that 30 bytes is actually a "header" of guitar pro file?
    }

    public async ValueTask<Gp5ScoreInformation> ReadScoreInformationAsync()
    {
        var scoreInformation = new Gp5ScoreInformation
        {
            Title = await _deserializer.ReadIntByteStringAsync(),
            Subtitle = await _deserializer.ReadIntByteStringAsync(),
            Artist = await _deserializer.ReadIntByteStringAsync(),
            Album = await _deserializer.ReadIntByteStringAsync(),
            Words = await _deserializer.ReadIntByteStringAsync(),
            Music = await _deserializer.ReadIntByteStringAsync(),
            Copyright = await _deserializer.ReadIntByteStringAsync(),
            Tab = await _deserializer.ReadIntByteStringAsync(),
            Instructions = await _deserializer.ReadIntByteStringAsync(),
            Notice = new string[await _deserializer.ReadIntAsync()]
        };

        for (var i = 0; i < scoreInformation.Notice.Length; i++)
        {
            var noticeLine = await _deserializer.ReadIntByteStringAsync();
            scoreInformation.Notice[i] = noticeLine;
        }

        return scoreInformation;
    }

    public async ValueTask<Gp5Lyrics> ReadLyricsAsync()
    {
        var lyrics = new Gp5Lyrics
        {
            ApplyToTrack = await _deserializer.ReadIntAsync(),
            FirstLine = await ReadLyricsLineAsync(),
            SecondLine = await ReadLyricsLineAsync(),
            ThirdLine = await ReadLyricsLineAsync(),
            FourthLine = await ReadLyricsLineAsync(),
            FifthLine = await ReadLyricsLineAsync()
        };

        return lyrics;
    }

    public async ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync()
    {
        const int rseMasterEffectEqualizerBandsCount = 10;
        var masterEffect = new Gp5RseMasterEffect
        {
            Volume = await _deserializer.ReadIntAsync(),
            _A01 = await _deserializer.ReadIntAsync(),
            Equalizer = await ReadRseEqualizerAsync(rseMasterEffectEqualizerBandsCount)
        };

        return masterEffect;
    }

    public async ValueTask<Gp5PageSetup> ReadPageSetupAsync()
    {
        var pageSetup = new Gp5PageSetup
        {
            Width = await _deserializer.ReadIntAsync(),
            Height = await _deserializer.ReadIntAsync(),
            MarginLeft = await _deserializer.ReadIntAsync(),
            MarginRight = await _deserializer.ReadIntAsync(),
            MarginTop = await _deserializer.ReadIntAsync(),
            MarginBottom = await _deserializer.ReadIntAsync(),
            ScoreSizeProportion = await _deserializer.ReadIntAsync(),
            HeaderAndFooterFlags = (Gp5PageSetup.HeaderAndFooter)await _deserializer.ReadShortAsync(),
            Title = await _deserializer.ReadIntByteStringAsync(),
            Subtitle = await _deserializer.ReadIntByteStringAsync(),
            Artist = await _deserializer.ReadIntByteStringAsync(),
            Album = await _deserializer.ReadIntByteStringAsync(),
            Words = await _deserializer.ReadIntByteStringAsync(),
            Music = await _deserializer.ReadIntByteStringAsync(),
            WordsAndMusic = await _deserializer.ReadIntByteStringAsync(),
            CopyrightFirstLine = await _deserializer.ReadIntByteStringAsync(),
            CopyrightSecondLine = await _deserializer.ReadIntByteStringAsync(),
            PageNumber = await _deserializer.ReadIntByteStringAsync()
        };

        return pageSetup;
    }

    public async ValueTask<Gp5Tempo> ReadHeaderTempoAsync()
    {
        var tempo = new Gp5Tempo
        {
            WordIndication = await _deserializer.ReadIntByteStringAsync(),
            BeatsPerMinute = await _deserializer.ReadIntAsync(),
            HideBeatsPerMinute = await _deserializer.ReadBoolAsync()
        };

        return tempo;
    }

    public async ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync()
    {
        var keySignature = new Gp5HeaderKeySignature
        {
            Key = await _deserializer.ReadSignedByteAsync(),
            _A01 = await _deserializer.ReadSignedByteAsync(),
            _A02 = await _deserializer.ReadSignedByteAsync(),
            _A03 = await _deserializer.ReadSignedByteAsync(),
            Octave = await _deserializer.ReadSignedByteAsync()
        };

        return keySignature;
    }

    public async ValueTask<Gp5MidiChannel> ReadMidiChannelAsync()
    {
        var midiChannel = new Gp5MidiChannel
        {
            Instrument = await _deserializer.ReadIntAsync(),
            Volume = await _deserializer.ReadByteAsync(),
            Balance = await _deserializer.ReadByteAsync(),
            Chorus = await _deserializer.ReadByteAsync(),
            Reverb = await _deserializer.ReadByteAsync(),
            Phaser = await _deserializer.ReadByteAsync(),
            Tremolo = await _deserializer.ReadByteAsync(),
            _A01 = await _deserializer.ReadByteAsync(),
            _A02 = await _deserializer.ReadByteAsync()
        };

        return midiChannel;
    }

    public async ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync()
    {
        var musicalDirections = new Gp5MusicalDirections
        {
            Coda = await _deserializer.ReadShortAsync(),
            DoubleCoda = await _deserializer.ReadShortAsync(),
            Segno = await _deserializer.ReadShortAsync(),
            SegnoSegno = await _deserializer.ReadShortAsync(),
            Fine = await _deserializer.ReadShortAsync(),
            DaCapo = await _deserializer.ReadShortAsync(),
            DaCapoAlCoda = await _deserializer.ReadShortAsync(),
            DaCapoAlDoubleCoda = await _deserializer.ReadShortAsync(),
            DaCapoAlFine = await _deserializer.ReadShortAsync(),
            DaSegno = await _deserializer.ReadShortAsync(),
            DaSegnoAlCoda = await _deserializer.ReadShortAsync(),
            DaSegnoAlDoubleCoda = await _deserializer.ReadShortAsync(),
            DaSegnoAlFine = await _deserializer.ReadShortAsync(),
            DaSegnoSegno = await _deserializer.ReadShortAsync(),
            DaSegnoSegnoAlCoda = await _deserializer.ReadShortAsync(),
            DaSegnoSegnoAlDoubleCoda = await _deserializer.ReadShortAsync(),
            DaSegnoSegnoAlFine = await _deserializer.ReadShortAsync(),
            DaCoda = await _deserializer.ReadShortAsync(),
            DaDoubleCoda = await _deserializer.ReadShortAsync()
        };

        return musicalDirections;
    }

    public ValueTask<int> ReadRseMasterEffectReverbAsync() =>
        _deserializer.ReadIntAsync();

    public ValueTask<int> ReadMeasuresCountAsync() =>
        _deserializer.ReadIntAsync();

    public ValueTask<int> ReadTracksCountAsync() =>
        _deserializer.ReadIntAsync();
}
