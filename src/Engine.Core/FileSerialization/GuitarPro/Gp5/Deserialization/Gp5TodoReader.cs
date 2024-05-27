using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5TodoReader : IGp5TodoReader
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;
    private readonly IGp5StringsReader _stringsReader;
    private readonly IGp5RseEqualizerReader _rseEqualizerReader;
    private readonly IGp5ColorReader _colorReader;

    public Gp5TodoReader(IGp5BinaryPrimitivesReader primitivesReader, IGp5StringsReader stringsReader,
        IGp5RseEqualizerReader rseEqualizerReader, IGp5ColorReader colorReader)
    {
        _primitivesReader = primitivesReader;
        _stringsReader = stringsReader;
        _rseEqualizerReader = rseEqualizerReader;
        _colorReader = colorReader;
    }

    public ValueTask<Gp5ByteString> ReadVersionAsync()
    {
        return _stringsReader.ReadByteStringAsync(Gp5File.VersionStringMaxLength);

        // TODO:
        // "version" data is stored in size of 30 bytes, the actual version string is 24 characters long
        // remaining 6 bytes seems to have some arbitrary data - it may be not just trailing string bytes
        // does that 30 bytes is actually a "header" of guitar pro file?
    }

    public async ValueTask<Gp5ScoreInformation> ReadScoreInformationAsync()
    {
        var scoreInformation = new Gp5ScoreInformation
        {
            Title = await _stringsReader.ReadIntByteStringAsync(),
            Subtitle = await _stringsReader.ReadIntByteStringAsync(),
            Artist = await _stringsReader.ReadIntByteStringAsync(),
            Album = await _stringsReader.ReadIntByteStringAsync(),
            Words = await _stringsReader.ReadIntByteStringAsync(),
            Music = await _stringsReader.ReadIntByteStringAsync(),
            Copyright = await _stringsReader.ReadIntByteStringAsync(),
            Tab = await _stringsReader.ReadIntByteStringAsync(),
            Instructions = await _stringsReader.ReadIntByteStringAsync(),
            Notice = new string[await _primitivesReader.ReadIntAsync()]
        };

        for (var i = 0; i < scoreInformation.Notice.Length; i++)
        {
            scoreInformation.Notice[i] = await _stringsReader.ReadIntByteStringAsync();
        }

        return scoreInformation;
    }

    public async ValueTask<Gp5Lyrics> ReadLyricsAsync()
    {
        return new Gp5Lyrics
        {
            ApplyToTrack = await _primitivesReader.ReadIntAsync(),
            FirstLine = await ReadLyricsLineAsync(),
            SecondLine = await ReadLyricsLineAsync(),
            ThirdLine = await ReadLyricsLineAsync(),
            FourthLine = await ReadLyricsLineAsync(),
            FifthLine = await ReadLyricsLineAsync()
        };
    }

    private async ValueTask<Gp5LyricsLine> ReadLyricsLineAsync()
    {
        return new Gp5LyricsLine
        {
            StartFromBar = await _primitivesReader.ReadIntAsync(),
            Lyrics = await _stringsReader.ReadIntStringAsync()
        };
    }

    public async ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync()
    {
        return new Gp5RseMasterEffect
        {
            Volume = await _primitivesReader.ReadIntAsync(),
            _A01 = await _primitivesReader.ReadIntAsync(),
            Equalizer = await _rseEqualizerReader.ReadRseEqualizerAsync(Gp5RseMasterEffect.EqualizerBandsCount)
        };
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
            Title = await _stringsReader.ReadIntByteStringAsync(),
            Subtitle = await _stringsReader.ReadIntByteStringAsync(),
            Artist = await _stringsReader.ReadIntByteStringAsync(),
            Album = await _stringsReader.ReadIntByteStringAsync(),
            Words = await _stringsReader.ReadIntByteStringAsync(),
            Music = await _stringsReader.ReadIntByteStringAsync(),
            WordsAndMusic = await _stringsReader.ReadIntByteStringAsync(),
            CopyrightFirstLine = await _stringsReader.ReadIntByteStringAsync(),
            CopyrightSecondLine = await _stringsReader.ReadIntByteStringAsync(),
            PageNumber = await _stringsReader.ReadIntByteStringAsync()
        };
    }

    public async ValueTask<Gp5Tempo> ReadHeaderTempoAsync()
    {
        return new Gp5Tempo
        {
            WordIndication = await _stringsReader.ReadIntByteStringAsync(),
            BeatsPerMinute = await _primitivesReader.ReadIntAsync(),
            HideBeatsPerMinute = await _primitivesReader.ReadBoolAsync()
        };
    }

    public async ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync()
    {
        return new Gp5HeaderKeySignature
        {
            Key = await _primitivesReader.ReadSignedByteAsync(),
            _A01 = await _primitivesReader.ReadSignedByteAsync(),
            _A02 = await _primitivesReader.ReadSignedByteAsync(),
            _A03 = await _primitivesReader.ReadSignedByteAsync(),
            Octave = await _primitivesReader.ReadSignedByteAsync()
        };
    }

    public async ValueTask<Gp5MidiChannel> ReadMidiChannelAsync()
    {
        return new Gp5MidiChannel
        {
            Instrument = await _primitivesReader.ReadIntAsync(),
            Volume = await _primitivesReader.ReadByteAsync(),
            Balance = await _primitivesReader.ReadByteAsync(),
            Chorus = await _primitivesReader.ReadByteAsync(),
            Reverb = await _primitivesReader.ReadByteAsync(),
            Phaser = await _primitivesReader.ReadByteAsync(),
            Tremolo = await _primitivesReader.ReadByteAsync(),
            _A01 = await _primitivesReader.ReadByteAsync(),
            _A02 = await _primitivesReader.ReadByteAsync()
        };
    }

    public async ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync()
    {
        return new Gp5MusicalDirections
        {
            Coda = await _primitivesReader.ReadShortAsync(),
            DoubleCoda = await _primitivesReader.ReadShortAsync(),
            Segno = await _primitivesReader.ReadShortAsync(),
            SegnoSegno = await _primitivesReader.ReadShortAsync(),
            Fine = await _primitivesReader.ReadShortAsync(),
            DaCapo = await _primitivesReader.ReadShortAsync(),
            DaCapoAlCoda = await _primitivesReader.ReadShortAsync(),
            DaCapoAlDoubleCoda = await _primitivesReader.ReadShortAsync(),
            DaCapoAlFine = await _primitivesReader.ReadShortAsync(),
            DaSegno = await _primitivesReader.ReadShortAsync(),
            DaSegnoAlCoda = await _primitivesReader.ReadShortAsync(),
            DaSegnoAlDoubleCoda = await _primitivesReader.ReadShortAsync(),
            DaSegnoAlFine = await _primitivesReader.ReadShortAsync(),
            DaSegnoSegno = await _primitivesReader.ReadShortAsync(),
            DaSegnoSegnoAlCoda = await _primitivesReader.ReadShortAsync(),
            DaSegnoSegnoAlDoubleCoda = await _primitivesReader.ReadShortAsync(),
            DaSegnoSegnoAlFine = await _primitivesReader.ReadShortAsync(),
            DaCoda = await _primitivesReader.ReadShortAsync(),
            DaDoubleCoda = await _primitivesReader.ReadShortAsync()
        };
    }

    public ValueTask<int> ReadRseMasterEffectReverbAsync() =>
        _primitivesReader.ReadIntAsync();

    public ValueTask<int> ReadMeasuresCountAsync() =>
        _primitivesReader.ReadIntAsync();

    public ValueTask<int> ReadTracksCountAsync() =>
        _primitivesReader.ReadIntAsync();

    public async ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst)
    {
        var primaryFlags = (Gp5MeasureHeader.Primary)await _primitivesReader.ReadByteAsync();
        var measureHeader = new Gp5MeasureHeader
        {
            PrimaryFlags = primaryFlags
        };

        var hasNumerator = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignatureNumerator);
        var hasDenominator = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignatureDenominator);
        var hasBeamGroups = hasNumerator || hasDenominator;
        measureHeader.TimeSignature = await ReadTimeSignatureAsync(hasNumerator: hasNumerator, hasDenominator: hasDenominator);

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasRepeatClose))
            measureHeader.RepeatCount = await _primitivesReader.ReadByteAsync();

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
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesReader.ReadByteAsync();
        }
        else
        {
            if (hasAlternateEndings)
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesReader.ReadByteAsync();

            if (hasBeamGroups)
                measureHeader.TimeSignature.BeamGroups = await ReadTimeSignatureBeamGroupsAsync();
        }

        if (!hasAlternateEndings)
            measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesReader.ReadByteAsync();

        measureHeader.TripletFeel = await _primitivesReader.ReadByteAsync();
        measureHeader.EndOfObjectSeparator = await _primitivesReader.ReadByteAsync();

        return measureHeader;
    }

    private async ValueTask<Gp5KeySignature> ReadKeySignatureAsync()
    {
        return new Gp5KeySignature
        {
            Key = await _primitivesReader.ReadSignedByteAsync(),
            IsMinorKey = await _primitivesReader.ReadBoolAsync()
        };
    }

    private async ValueTask<Gp5TimeSignature> ReadTimeSignatureAsync(bool hasNumerator, bool hasDenominator)
    {
        if (!hasNumerator && !hasDenominator)
            return null;

        return new Gp5TimeSignature
        {
            Numerator = hasNumerator ? await _primitivesReader.ReadByteAsync() : null,
            Denominator = hasDenominator ? await _primitivesReader.ReadByteAsync() : null
        };
    }

    private async ValueTask<Gp5TimeSignatureBeamGroups> ReadTimeSignatureBeamGroupsAsync()
    {
        return new Gp5TimeSignatureBeamGroups
        {
            FirstSpan = await _primitivesReader.ReadByteAsync(),
            SecondSpan = await _primitivesReader.ReadByteAsync(),
            ThirdSpan = await _primitivesReader.ReadByteAsync(),
            FourthSpan = await _primitivesReader.ReadByteAsync()
        };
    }

    private async ValueTask<Gp5Marker> ReadMarkerAsync()
    {
        return new Gp5Marker
        {
            Name = await _stringsReader.ReadIntByteStringAsync(),
            Color = await _colorReader.ReadColorAsync()
        };
    }
}
