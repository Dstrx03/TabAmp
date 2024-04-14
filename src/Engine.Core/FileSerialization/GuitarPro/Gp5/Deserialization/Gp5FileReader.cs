using System;
using System.Buffers.Binary;
using System.Text;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Wrappers;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5FileReader
{
    private readonly ISerialFileReader _fileReader;

    public Gp5FileReader(ISerialFileReader fileReader) =>
        _fileReader = fileReader;

    public virtual ValueTask<string> ReadVersionAsync()
    {
        return ReadByteStringAsync(Gp5File.VersionStringMaxLength);

        // TODO:
        // "version" data is stored in size of 30 bytes, the actual version string is 24 characters long
        // remaining 6 bytes seems to have some arbitrary data - it may be not just trailing string bytes
        // does that 30 bytes is actually a "header" of guitar pro file?
    }

    public virtual async ValueTask<Gp5ScoreInformation> ReadScoreInformationAsync()
    {
        var scoreInformation = new Gp5ScoreInformation
        {
            Title = await ReadIntByteStringAsync(),
            Subtitle = await ReadIntByteStringAsync(),
            Artist = await ReadIntByteStringAsync(),
            Album = await ReadIntByteStringAsync(),
            Words = await ReadIntByteStringAsync(),
            Music = await ReadIntByteStringAsync(),
            Copyright = await ReadIntByteStringAsync(),
            Tab = await ReadIntByteStringAsync(),
            Instructions = await ReadIntByteStringAsync(),
            Notice = new string[await ReadIntAsync()]
        };

        for (var i = 0; i < scoreInformation.Notice.Length; i++)
        {
            var noticeLine = await ReadIntByteStringAsync();
            scoreInformation.Notice[i] = noticeLine;
        }

        return scoreInformation;
    }

    public virtual async ValueTask<Gp5Lyrics> ReadLyricsAsync()
    {
        return new Gp5Lyrics
        {
            ApplyToTrack = await ReadIntAsync(),
            FirstLine = await ReadLyricsLineAsync(),
            SecondLine = await ReadLyricsLineAsync(),
            ThirdLine = await ReadLyricsLineAsync(),
            FourthLine = await ReadLyricsLineAsync(),
            FifthLine = await ReadLyricsLineAsync()
        };
    }

    public virtual async ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync()
    {
        return new Gp5RseMasterEffect
        {
            Volume = await ReadIntAsync(),
            _A01 = await ReadIntAsync(),
            Equalizer = await ReadRseEqualizerAsync(Gp5RseMasterEffect.EqualizerBandsCount)
        };
    }

    public virtual async ValueTask<Gp5PageSetup> ReadPageSetupAsync()
    {
        return new Gp5PageSetup
        {
            Width = await ReadIntAsync(),
            Height = await ReadIntAsync(),
            MarginLeft = await ReadIntAsync(),
            MarginRight = await ReadIntAsync(),
            MarginTop = await ReadIntAsync(),
            MarginBottom = await ReadIntAsync(),
            ScoreSizeProportion = await ReadIntAsync(),
            HeaderAndFooterFlags = (Gp5PageSetup.HeaderAndFooter)await ReadShortAsync(),
            Title = await ReadIntByteStringAsync(),
            Subtitle = await ReadIntByteStringAsync(),
            Artist = await ReadIntByteStringAsync(),
            Album = await ReadIntByteStringAsync(),
            Words = await ReadIntByteStringAsync(),
            Music = await ReadIntByteStringAsync(),
            WordsAndMusic = await ReadIntByteStringAsync(),
            CopyrightFirstLine = await ReadIntByteStringAsync(),
            CopyrightSecondLine = await ReadIntByteStringAsync(),
            PageNumber = await ReadIntByteStringAsync()
        };
    }

    public virtual async ValueTask<Gp5Tempo> ReadHeaderTempoAsync()
    {
        return new Gp5Tempo
        {
            WordIndication = await ReadIntByteStringAsync(),
            BeatsPerMinute = await ReadIntAsync(),
            HideBeatsPerMinute = await ReadBooleanAsync()
        };
    }

    public virtual async ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync()
    {
        return new Gp5HeaderKeySignature
        {
            Key = await ReadSignedByteAsync(),
            _A01 = await ReadSignedByteAsync(),
            _A02 = await ReadSignedByteAsync(),
            _A03 = await ReadSignedByteAsync(),
            Octave = await ReadSignedByteAsync()
        };
    }

    public virtual async ValueTask<Gp5MidiChannel> ReadMidiChannelAsync()
    {
        return new Gp5MidiChannel
        {
            Instrument = await ReadIntAsync(),
            Volume = await ReadByteAsync(),
            Balance = await ReadByteAsync(),
            Chorus = await ReadByteAsync(),
            Reverb = await ReadByteAsync(),
            Phaser = await ReadByteAsync(),
            Tremolo = await ReadByteAsync(),
            _A01 = await ReadByteAsync(),
            _A02 = await ReadByteAsync()
        };
    }

    public virtual async ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync()
    {
        return new Gp5MusicalDirections
        {
            Coda = await ReadShortAsync(),
            DoubleCoda = await ReadShortAsync(),
            Segno = await ReadShortAsync(),
            SegnoSegno = await ReadShortAsync(),
            Fine = await ReadShortAsync(),
            DaCapo = await ReadShortAsync(),
            DaCapoAlCoda = await ReadShortAsync(),
            DaCapoAlDoubleCoda = await ReadShortAsync(),
            DaCapoAlFine = await ReadShortAsync(),
            DaSegno = await ReadShortAsync(),
            DaSegnoAlCoda = await ReadShortAsync(),
            DaSegnoAlDoubleCoda = await ReadShortAsync(),
            DaSegnoAlFine = await ReadShortAsync(),
            DaSegnoSegno = await ReadShortAsync(),
            DaSegnoSegnoAlCoda = await ReadShortAsync(),
            DaSegnoSegnoAlDoubleCoda = await ReadShortAsync(),
            DaSegnoSegnoAlFine = await ReadShortAsync(),
            DaCoda = await ReadShortAsync(),
            DaDoubleCoda = await ReadShortAsync()
        };
    }

    public virtual ValueTask<int> ReadRseMasterEffectReverbAsync() =>
        ReadIntAsync();

    public virtual ValueTask<int> ReadMeasuresCountAsync() =>
        ReadIntAsync();

    public virtual ValueTask<int> ReadTracksCountAsync() =>
        ReadIntAsync();

    public virtual async ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst)
    {
        var primaryFlags = (Gp5MeasureHeader.Primary)await ReadByteAsync();
        var measureHeader = new Gp5MeasureHeader
        {
            PrimaryFlags = primaryFlags
        };

        var hasNumerator = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignatureNumerator);
        var hasDenominator = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignatureDenominator);
        var hasBeamGroups = hasNumerator || hasDenominator;
        measureHeader.TimeSignature = await ReadTimeSignatureAsync(hasNumerator: hasNumerator, hasDenominator: hasDenominator);

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasRepeatClose))
            measureHeader.RepeatCount = await ReadByteAsync();

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
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await ReadByteAsync();
        }
        else
        {
            if (hasAlternateEndings)
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await ReadByteAsync();

            if (hasBeamGroups)
                measureHeader.TimeSignature.BeamGroups = await ReadTimeSignatureBeamGroupsAsync();
        }

        if (!hasAlternateEndings)
            measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await ReadByteAsync();

        measureHeader.TripletFeel = await ReadByteAsync();
        measureHeader.EndOfObjectSeparator = await ReadByteAsync();

        return measureHeader;
    }

    protected virtual async ValueTask<Gp5LyricsLine> ReadLyricsLineAsync()
    {
        return new Gp5LyricsLine
        {
            StartFromBar = await ReadIntAsync(),
            Lyrics = await ReadIntStringAsync()
        };
    }

    protected virtual async ValueTask<Gp5KeySignature> ReadKeySignatureAsync()
    {
        return new Gp5KeySignature
        {
            Key = await ReadSignedByteAsync(),
            IsMinorKey = await ReadBooleanAsync()
        };
    }

    protected virtual async ValueTask<Gp5TimeSignature> ReadTimeSignatureAsync(bool hasNumerator, bool hasDenominator)
    {
        if (!hasNumerator && !hasDenominator)
            return null;

        return new Gp5TimeSignature
        {
            Numerator = hasNumerator ? await ReadByteAsync() : null,
            Denominator = hasDenominator ? await ReadByteAsync() : null
        };
    }

    protected virtual async ValueTask<Gp5TimeSignatureBeamGroups> ReadTimeSignatureBeamGroupsAsync()
    {
        return new Gp5TimeSignatureBeamGroups
        {
            FirstSpan = await ReadByteAsync(),
            SecondSpan = await ReadByteAsync(),
            ThirdSpan = await ReadByteAsync(),
            FourthSpan = await ReadByteAsync()
        };
    }

    protected virtual async ValueTask<Gp5Marker> ReadMarkerAsync()
    {
        return new Gp5Marker
        {
            Name = await ReadIntByteStringAsync(),
            Color = await ReadColorAsync()
        };
    }

    protected virtual async ValueTask<Gp5RseEqualizer> ReadRseEqualizerAsync(int bandsCount)
    {
        var bands = new sbyte[bandsCount];
        for (var i = 0; i < bands.Length; i++)
        {
            bands[i] = await ReadSignedByteAsync();
        }

        var gainPreFader = await ReadSignedByteAsync();

        return new Gp5RseEqualizer
        {
            Bands = bands,
            GainPreFader = gainPreFader
        };
    }

    protected virtual async ValueTask<Gp5Color> ReadColorAsync()
    {
        return new Gp5Color
        {
            Red = await ReadByteAsync(),
            Green = await ReadByteAsync(),
            Blue = await ReadByteAsync(),
            Alpha = await ReadByteAsync()
        };
    }

    protected virtual async ValueTask<string> ReadStringAsync(int length)
    {
        var buffer = await _fileReader.ReadBytesAsync(length);
        return Encoding.UTF8.GetString(buffer);
    }

    protected virtual async ValueTask<string> ReadByteStringAsync(int maxLength)
    {
        var length = await ReadByteAsync();
        var decodedString = await ReadStringAsync(length);

        var trailingBytesCount = maxLength - length;
        if (trailingBytesCount > 0)
            await _fileReader.SkipBytesAsync(trailingBytesCount);
        else if (trailingBytesCount < 0)
            // TODO: more specific exception type, message
            throw new InvalidOperationException($"{maxLength}-{length}<0 P={_fileReader.Position}");

        return decodedString;
    }

    protected virtual async ValueTask<string> ReadIntStringAsync()
    {
        var length = await ReadIntAsync();
        return await ReadStringAsync(length);
    }

    protected virtual async ValueTask<string> ReadIntByteStringAsync()
    {
        var maxLength = await ReadIntAsync();
        var length = await ReadByteAsync();

        if (length + ByteStringLengthPrefixSize != maxLength)
            // TODO: more specific exception type, message
            throw new InvalidOperationException($"{length}+{ByteStringLengthPrefixSize}!={maxLength} P={_fileReader.Position}");

        return await ReadStringAsync(length);
    }

    // TODO: move consts to more appropriate place(s)
    private const int ByteSize = 1;
    private const int ShortSize = 2;
    private const int IntSize = 4;
    private const int FloatSize = 4;
    private const int DoubleSize = 8;

    private const int ByteStringLengthPrefixSize = ByteSize;
    // TODO: move consts to more appropriate place(s)

    protected virtual async ValueTask<byte> ReadByteAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(ByteSize);
        return buffer[0];
    }

    protected virtual async ValueTask<sbyte> ReadSignedByteAsync()
    {
        var byteValue = await ReadByteAsync();
        return (sbyte)byteValue;
    }

    protected virtual async ValueTask<Gp5Boolean> ReadBooleanAsync()
    {
        var byteValue = await ReadByteAsync();
        return new Gp5Boolean(byteValue);

        // TODO: move to appropriate level
        /*if (byteValue != BoolFalseValue && byteValue != BoolTrueValue)
            // TODO: more specific exception type, message
            throw new InvalidOperationException($"{byteValue}!=0<>1 P={_fileReader.Position}");

        return byteValue == BoolTrueValue;*/
    }

    protected virtual async ValueTask<short> ReadShortAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(ShortSize);
        return BinaryPrimitives.ReadInt16LittleEndian(buffer);
    }

    protected virtual async ValueTask<int> ReadIntAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(IntSize);
        return BinaryPrimitives.ReadInt32LittleEndian(buffer);
    }

    protected virtual async ValueTask<float> ReadFloatAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(FloatSize);
        return BinaryPrimitives.ReadSingleLittleEndian(buffer);
    }

    protected virtual async ValueTask<double> ReadDoubleAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(DoubleSize);
        return BinaryPrimitives.ReadDoubleLittleEndian(buffer);
    }
}
