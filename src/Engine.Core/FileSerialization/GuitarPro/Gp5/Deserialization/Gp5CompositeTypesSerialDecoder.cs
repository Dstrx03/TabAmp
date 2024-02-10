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

    public async ValueTask<string> ReadVersionAsync()
    {
        const int versionStringMaxLength = 30;
        var versionString = await ReadByteStringAsync(versionStringMaxLength);

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
            Title = await ReadIntByteStringAsync(),
            Subtitle = await ReadIntByteStringAsync(),
            Artist = await ReadIntByteStringAsync(),
            Album = await ReadIntByteStringAsync(),
            Words = await ReadIntByteStringAsync(),
            Music = await ReadIntByteStringAsync(),
            Copyright = await ReadIntByteStringAsync(),
            Tab = await ReadIntByteStringAsync(),
            Instructions = await ReadIntByteStringAsync(),
            Notice = new string[await _primitivesDecoder.ReadIntAsync()]
        };

        for (var i = 0; i < scoreInformation.Notice.Length; i++)
        {
            var noticeLine = await ReadIntByteStringAsync();
            scoreInformation.Notice[i] = noticeLine;
        }

        return scoreInformation;
    }

    public async ValueTask<Gp5Lyrics> ReadLyricsAsync()
    {
        var lyrics = new Gp5Lyrics
        {
            ApplyToTrack = await _primitivesDecoder.ReadIntAsync(),
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
            Volume = await _primitivesDecoder.ReadIntAsync(),
            _A01 = await _primitivesDecoder.ReadIntAsync(),
            Equalizer = await ReadRseEqualizerAsync(rseMasterEffectEqualizerBandsCount)
        };

        return masterEffect;
    }

    public async ValueTask<Gp5PageSetup> ReadPageSetupAsync()
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

        return pageSetup;
    }

    public async ValueTask<Gp5Tempo> ReadHeaderTempoAsync()
    {
        var tempo = new Gp5Tempo
        {
            WordIndication = await ReadIntByteStringAsync(),
            BeatsPerMinute = await _primitivesDecoder.ReadIntAsync(),
            HideBeatsPerMinute = await _primitivesDecoder.ReadBoolAsync()
        };

        return tempo;
    }

    public async ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync()
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

    public async ValueTask<Gp5MidiChannel> ReadMidiChannelAsync()
    {
        var midiChannel = new Gp5MidiChannel
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

        return midiChannel;
    }

    public async ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync()
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
}
