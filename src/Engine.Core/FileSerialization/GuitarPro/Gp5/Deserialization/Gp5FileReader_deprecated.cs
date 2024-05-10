using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5FileReader_deprecated
{
    private readonly Gp5TypesReader_deprecated _reader;

    public Gp5FileReader_deprecated(Gp5TypesReader_deprecated reader) =>
        _reader = reader;

    public ValueTask<string> ReadVersionAsync()
    {
        return _reader.ReadByteStringAsync(Gp5File.VersionStringMaxLength);

        // TODO:
        // "version" data is stored in size of 30 bytes, the actual version string is 24 characters long
        // remaining 6 bytes seems to have some arbitrary data - it may be not just trailing string bytes
        // does that 30 bytes is actually a "header" of guitar pro file?
    }

    public async ValueTask<Gp5ScoreInformation> ReadScoreInformationAsync()
    {
        var scoreInformation = new Gp5ScoreInformation
        {
            Title = await _reader.ReadIntByteStringAsync(),
            Subtitle = await _reader.ReadIntByteStringAsync(),
            Artist = await _reader.ReadIntByteStringAsync(),
            Album = await _reader.ReadIntByteStringAsync(),
            Words = await _reader.ReadIntByteStringAsync(),
            Music = await _reader.ReadIntByteStringAsync(),
            Copyright = await _reader.ReadIntByteStringAsync(),
            Tab = await _reader.ReadIntByteStringAsync(),
            Instructions = await _reader.ReadIntByteStringAsync(),
            Notice = new string[await _reader.ReadIntAsync()]
        };

        for (var i = 0; i < scoreInformation.Notice.Length; i++)
        {
            var noticeLine = await _reader.ReadIntByteStringAsync();
            scoreInformation.Notice[i] = noticeLine;
        }

        return scoreInformation;
    }

    public async ValueTask<Gp5Lyrics> ReadLyricsAsync()
    {
        return new Gp5Lyrics
        {
            ApplyToTrack = await _reader.ReadIntAsync(),
            FirstLine = await ReadLyricsLineAsync(),
            SecondLine = await ReadLyricsLineAsync(),
            ThirdLine = await ReadLyricsLineAsync(),
            FourthLine = await ReadLyricsLineAsync(),
            FifthLine = await ReadLyricsLineAsync()
        };
    }

    public async ValueTask<Gp5LyricsLine> ReadLyricsLineAsync()
    {
        return new Gp5LyricsLine
        {
            StartFromBar = await _reader.ReadIntAsync(),
            Lyrics = await _reader.ReadIntStringAsync()
        };
    }

    public async ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync()
    {
        return new Gp5RseMasterEffect
        {
            Volume = await _reader.ReadIntAsync(),
            _A01 = await _reader.ReadIntAsync(),
            Equalizer = await ReadRseEqualizerAsync(Gp5RseMasterEffect.EqualizerBandsCount)
        };
    }

    public async ValueTask<Gp5PageSetup> ReadPageSetupAsync()
    {
        return new Gp5PageSetup
        {
            Width = await _reader.ReadIntAsync(),
            Height = await _reader.ReadIntAsync(),
            MarginLeft = await _reader.ReadIntAsync(),
            MarginRight = await _reader.ReadIntAsync(),
            MarginTop = await _reader.ReadIntAsync(),
            MarginBottom = await _reader.ReadIntAsync(),
            ScoreSizeProportion = await _reader.ReadIntAsync(),
            HeaderAndFooterFlags = (Gp5PageSetup.HeaderAndFooter)await _reader.ReadShortAsync(),
            Title = await _reader.ReadIntByteStringAsync(),
            Subtitle = await _reader.ReadIntByteStringAsync(),
            Artist = await _reader.ReadIntByteStringAsync(),
            Album = await _reader.ReadIntByteStringAsync(),
            Words = await _reader.ReadIntByteStringAsync(),
            Music = await _reader.ReadIntByteStringAsync(),
            WordsAndMusic = await _reader.ReadIntByteStringAsync(),
            CopyrightFirstLine = await _reader.ReadIntByteStringAsync(),
            CopyrightSecondLine = await _reader.ReadIntByteStringAsync(),
            PageNumber = await _reader.ReadIntByteStringAsync()
        };
    }

    public async ValueTask<Gp5Tempo> ReadHeaderTempoAsync()
    {
        return new Gp5Tempo
        {
            WordIndication = await _reader.ReadIntByteStringAsync(),
            BeatsPerMinute = await _reader.ReadIntAsync(),
            HideBeatsPerMinute = await _reader.ReadBoolAsync()
        };
    }

    public async ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync()
    {
        return new Gp5HeaderKeySignature
        {
            Key = await _reader.ReadSignedByteAsync(),
            _A01 = await _reader.ReadSignedByteAsync(),
            _A02 = await _reader.ReadSignedByteAsync(),
            _A03 = await _reader.ReadSignedByteAsync(),
            Octave = await _reader.ReadSignedByteAsync()
        };
    }

    public async ValueTask<Gp5MidiChannel> ReadMidiChannelAsync()
    {
        return new Gp5MidiChannel
        {
            Instrument = await _reader.ReadIntAsync(),
            Volume = await _reader.ReadByteAsync(),
            Balance = await _reader.ReadByteAsync(),
            Chorus = await _reader.ReadByteAsync(),
            Reverb = await _reader.ReadByteAsync(),
            Phaser = await _reader.ReadByteAsync(),
            Tremolo = await _reader.ReadByteAsync(),
            _A01 = await _reader.ReadByteAsync(),
            _A02 = await _reader.ReadByteAsync()
        };
    }

    public async ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync()
    {
        return new Gp5MusicalDirections
        {
            Coda = await _reader.ReadShortAsync(),
            DoubleCoda = await _reader.ReadShortAsync(),
            Segno = await _reader.ReadShortAsync(),
            SegnoSegno = await _reader.ReadShortAsync(),
            Fine = await _reader.ReadShortAsync(),
            DaCapo = await _reader.ReadShortAsync(),
            DaCapoAlCoda = await _reader.ReadShortAsync(),
            DaCapoAlDoubleCoda = await _reader.ReadShortAsync(),
            DaCapoAlFine = await _reader.ReadShortAsync(),
            DaSegno = await _reader.ReadShortAsync(),
            DaSegnoAlCoda = await _reader.ReadShortAsync(),
            DaSegnoAlDoubleCoda = await _reader.ReadShortAsync(),
            DaSegnoAlFine = await _reader.ReadShortAsync(),
            DaSegnoSegno = await _reader.ReadShortAsync(),
            DaSegnoSegnoAlCoda = await _reader.ReadShortAsync(),
            DaSegnoSegnoAlDoubleCoda = await _reader.ReadShortAsync(),
            DaSegnoSegnoAlFine = await _reader.ReadShortAsync(),
            DaCoda = await _reader.ReadShortAsync(),
            DaDoubleCoda = await _reader.ReadShortAsync()
        };
    }

    public ValueTask<int> ReadRseMasterEffectReverbAsync() =>
        _reader.ReadIntAsync();

    public ValueTask<int> ReadMeasuresCountAsync() =>
        _reader.ReadIntAsync();

    public ValueTask<int> ReadTracksCountAsync() =>
        _reader.ReadIntAsync();

    public async ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst)
    {
        var primaryFlags = (Gp5MeasureHeader.Primary)await _reader.ReadByteAsync();
        var measureHeader = new Gp5MeasureHeader
        {
            PrimaryFlags = primaryFlags
        };

        var hasNumerator = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignatureNumerator);
        var hasDenominator = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignatureDenominator);
        var hasBeamGroups = hasNumerator || hasDenominator;
        measureHeader.TimeSignature = await ReadTimeSignatureAsync(hasNumerator: hasNumerator, hasDenominator: hasDenominator);

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasRepeatClose))
            measureHeader.RepeatCount = await _reader.ReadByteAsync();

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
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _reader.ReadByteAsync();
        }
        else
        {
            if (hasAlternateEndings)
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _reader.ReadByteAsync();

            if (hasBeamGroups)
                measureHeader.TimeSignature.BeamGroups = await ReadTimeSignatureBeamGroupsAsync();
        }

        if (!hasAlternateEndings)
            measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _reader.ReadByteAsync();

        measureHeader.TripletFeel = await _reader.ReadByteAsync();
        measureHeader.EndOfObjectSeparator = await _reader.ReadByteAsync();

        return measureHeader;
    }

    public async ValueTask<Gp5KeySignature> ReadKeySignatureAsync()
    {
        return new Gp5KeySignature
        {
            Key = await _reader.ReadSignedByteAsync(),
            IsMinorKey = await _reader.ReadBoolAsync()
        };
    }

    public async ValueTask<Gp5TimeSignature> ReadTimeSignatureAsync(bool hasNumerator, bool hasDenominator)
    {
        if (!hasNumerator && !hasDenominator)
            return null;

        return new Gp5TimeSignature
        {
            Numerator = hasNumerator ? await _reader.ReadByteAsync() : null,
            Denominator = hasDenominator ? await _reader.ReadByteAsync() : null
        };
    }

    public async ValueTask<Gp5TimeSignatureBeamGroups> ReadTimeSignatureBeamGroupsAsync()
    {
        return new Gp5TimeSignatureBeamGroups
        {
            FirstSpan = await _reader.ReadByteAsync(),
            SecondSpan = await _reader.ReadByteAsync(),
            ThirdSpan = await _reader.ReadByteAsync(),
            FourthSpan = await _reader.ReadByteAsync()
        };
    }

    public async ValueTask<Gp5Marker> ReadMarkerAsync()
    {
        return new Gp5Marker
        {
            Name = await _reader.ReadIntByteStringAsync(),
            Color = await ReadColorAsync()
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
