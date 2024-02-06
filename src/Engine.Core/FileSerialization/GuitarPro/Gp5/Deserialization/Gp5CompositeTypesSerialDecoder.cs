using System;
using System.Text;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5CompositeTypesSerialDecoder
{
    private readonly ISerialFileReader _fileReader;
    private readonly Gp5PrimitivesSerialDecoder _primitivesDecoder;

    public Gp5CompositeTypesSerialDecoder(ISerialFileReader fileReader, Gp5PrimitivesSerialDecoder primitivesDecoder)
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
        var primaryFlags = (Gp5MeasureHeader.Primary)await _primitivesDecoder.ReadByteAsync();
        var measureHeader = new Gp5MeasureHeader
        {
            PrimaryFlags = primaryFlags
        };

        var hasNumerator = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignatureNumerator);
        var hasDenominator = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignatureDenominator);
        var hasBeamGroups = hasNumerator || hasDenominator;
        measureHeader.TimeSignature = await ReadTimeSignatureAsync(hasNumerator: hasNumerator, hasDenominator: hasDenominator);

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasRepeatClose))
            measureHeader.RepeatCount = await _primitivesDecoder.ReadByteAsync();

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
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesDecoder.ReadByteAsync();
        }
        else
        {
            if (hasAlternateEndings)
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesDecoder.ReadByteAsync();

            if (hasBeamGroups)
                measureHeader.TimeSignature.BeamGroups = await ReadTimeSignatureBeamGroupsAsync();
        }

        if (!hasAlternateEndings)
            measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesDecoder.ReadByteAsync();

        measureHeader.TripletFeel = await _primitivesDecoder.ReadByteAsync();
        measureHeader.EndOfObjectSeparator = await _primitivesDecoder.ReadByteAsync();

        return measureHeader;
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
