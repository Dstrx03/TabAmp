using System;
using System.Text;
using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat.FileReader;
using TabAmp.Engine.GuitarProFileFormat.Models;

namespace TabAmp.Engine.GuitarProFileFormat;

internal class Gp5CompositeTypesSerialDecoder
{
    private readonly ISerialAsynchronousFileReader _fileReader;
    private readonly Gp5PrimitivesSerialDecoder _primitivesDecoder;

    public Gp5CompositeTypesSerialDecoder(ISerialAsynchronousFileReader fileReader, Gp5PrimitivesSerialDecoder primitivesDecoder)
    {
        _fileReader = fileReader;
        _primitivesDecoder = primitivesDecoder;
    }

    public async ValueTask<string> ReadByteStringAsync(int maxLength)
    {
        var length = await _primitivesDecoder.ReadByteAsync();
        var decodedString = await ReadStringAsync(length);

        var trailingBytesCount = maxLength - length;
        if (trailingBytesCount > 0)
            await _fileReader.SkipBytesAsync(trailingBytesCount);
        else if (trailingBytesCount < 0)
            // TODO: more specific exception type, message
            throw new InvalidOperationException($"{maxLength}-{length}<0 P={_fileReader.Position}");

        return decodedString;
    }

    public async ValueTask<string> ReadIntStringAsync()
    {
        var length = await _primitivesDecoder.ReadIntAsync();
        return await ReadStringAsync(length);
    }

    public async ValueTask<string> ReadIntByteStringAsync()
    {
        const int lengthPrefixSize = Gp5PrimitivesSerialDecoder.ByteSize;

        var maxLength = await _primitivesDecoder.ReadIntAsync();
        var length = await _primitivesDecoder.ReadByteAsync();

        if (length + lengthPrefixSize != maxLength)
            // TODO: more specific exception type, message
            throw new InvalidOperationException($"{length}+{lengthPrefixSize}!={maxLength} P={_fileReader.Position}");

        return await ReadStringAsync(length);
    }

    private async ValueTask<string> ReadStringAsync(int length)
    {
        var buffer = await _fileReader.ReadBytesAsync(length);
        return Encoding.UTF8.GetString(buffer);
    }

    public async ValueTask<Gp5RseEqualizer> ReadRseEqualizerAsync(int bandsCount)
    {
        var bands = new sbyte[bandsCount];
        for (var i = 0; i < bands.Length; i++)
        {
            bands[i] = await _primitivesDecoder.ReadSignedByteAsync();
        }

        var gainPreFader = await _primitivesDecoder.ReadSignedByteAsync();

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
            Numerator = hasNumerator ? await _primitivesDecoder.ReadByteAsync() : null,
            Denominator = hasDenominator ? await _primitivesDecoder.ReadByteAsync() : null
        };
    }

    public async ValueTask<Gp5TimeSignatureBeamGroups> ReadTimeSignatureBeamGroupsAsync()
    {
        return new Gp5TimeSignatureBeamGroups
        {
            FirstSpan = await _primitivesDecoder.ReadByteAsync(),
            SecondSpan = await _primitivesDecoder.ReadByteAsync(),
            ThirdSpan = await _primitivesDecoder.ReadByteAsync(),
            FourthSpan = await _primitivesDecoder.ReadByteAsync()
        };
    }

    public async ValueTask<Gp5Marker> ReadMarkerAsync()
    {
        return new Gp5Marker
        {
            Name = await ReadIntByteStringAsync(),
            Color = await ReadColorAsync()
        };
    }

    public async ValueTask<Gp5Color> ReadColorAsync()
    {
        return new Gp5Color
        {
            Red = await _primitivesDecoder.ReadByteAsync(),
            Green = await _primitivesDecoder.ReadByteAsync(),
            Blue = await _primitivesDecoder.ReadByteAsync(),
            Alpha = await _primitivesDecoder.ReadByteAsync()
        };
    }

    public async ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst)
    {
        var measureHeader = new Gp5MeasureHeader();

        if (!isFirst)
            measureHeader.FirstBlankTodo = await _primitivesDecoder.ReadByteAsync();

        measureHeader.Flags = (Gp5MeasureHeader.MeasureHeaderFlags)await _primitivesDecoder.ReadByteAsync();
        var flags = measureHeader.Flags;

        await ReadTimeSignatureIfAnyAsync();
        await ReadRepeatCountIfAnyAsync();
        await ReadMarkerIfAnyAsync();
        await ReadKeySignatureIfAnyAsync();

        if (!isFirst)
        {
            await ReadAlternateEndingsIfAnyAsync();
            await ReadTimeSignatureBeamGroupsIfAnyAsync();
        }
        else
        {
            await ReadTimeSignatureBeamGroupsIfAnyAsync();
            await ReadAlternateEndingsIfAnyAsync();
        }

        await ReadSecondBlankTodoIfAnyAsync();

        measureHeader.TripletFeel = await _primitivesDecoder.ReadByteAsync();

        return measureHeader;

        async ValueTask ReadTimeSignatureIfAnyAsync()
        {
            var hasNumerator = flags.HasFlag(Gp5MeasureHeader.MeasureHeaderFlags.HasTimeSignatureNumerator);
            var hasDenominator = flags.HasFlag(Gp5MeasureHeader.MeasureHeaderFlags.HasTimeSignatureDenominator);
            measureHeader.TimeSignature = await ReadTimeSignatureAsync(hasNumerator: hasNumerator, hasDenominator: hasDenominator);
        }

        async ValueTask ReadRepeatCountIfAnyAsync()
        {
            if (flags.HasFlag(Gp5MeasureHeader.MeasureHeaderFlags.HasRepeatClose))
                measureHeader.RepeatCount = await _primitivesDecoder.ReadByteAsync();
        }

        async ValueTask ReadMarkerIfAnyAsync()
        {
            if (flags.HasFlag(Gp5MeasureHeader.MeasureHeaderFlags.HasMarker))
                measureHeader.Marker = await ReadMarkerAsync();
        }

        async ValueTask ReadKeySignatureIfAnyAsync()
        {
            if (flags.HasFlag(Gp5MeasureHeader.MeasureHeaderFlags.HasKeySignature))
                measureHeader.KeySignature = await ReadKeySignatureAsync();
        }

        async ValueTask ReadAlternateEndingsIfAnyAsync()
        {
            if (flags.HasFlag(Gp5MeasureHeader.MeasureHeaderFlags.HasAlternateEndings))
                measureHeader.AlternateEndings = (Gp5MeasureHeader.AlternateEndingsFlags)await _primitivesDecoder.ReadByteAsync();
        }

        async ValueTask ReadTimeSignatureBeamGroupsIfAnyAsync()
        {
            if (measureHeader.TimeSignature is not null)
                measureHeader.TimeSignature.BeamGroups = await ReadTimeSignatureBeamGroupsAsync();
        }

        async ValueTask ReadSecondBlankTodoIfAnyAsync()
        {
            if (!flags.HasFlag(Gp5MeasureHeader.MeasureHeaderFlags.HasAlternateEndings))
                measureHeader.SecondBlankTodo = await _primitivesDecoder.ReadByteAsync();
        }
    }

    public async ValueTask<Gp5KeySignature> ReadKeySignatureAsync()
    {
        return new Gp5KeySignature
        {
            Key = await _primitivesDecoder.ReadSignedByteAsync(),
            IsMinorKey = await _primitivesDecoder.ReadBoolAsync()
        };
    }

    public async ValueTask<Gp5LyricsLine> ReadLyricsLineAsync()
    {
        return new Gp5LyricsLine
        {
            StartFromBar = await _primitivesDecoder.ReadIntAsync(),
            Lyrics = await ReadIntStringAsync()
        };
    }
}
