using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Strings;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5TodoReaderIntegrityValidator : IGp5TodoReader
{
    private readonly IGp5TodoReader _reader;

    public Gp5TodoReaderIntegrityValidator(IGp5TodoReader reader) =>
        _reader = reader;

    public ValueTask<Gp5ByteString> ReadVersionAsync() =>
        _reader.ReadVersionAsync();

    public ValueTask<Gp5ScoreInformation> ReadScoreInformationAsync() =>
        _reader.ReadScoreInformationAsync();

    public ValueTask<Gp5Lyrics> ReadLyricsAsync() =>
        _reader.ReadLyricsAsync();

    public async ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync()
    {
        var rseMasterEffect = await _reader.ReadRseMasterEffectAsync();

        if (rseMasterEffect._A01 != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"expected to be 0: _A01={rseMasterEffect._A01}");

        return rseMasterEffect;
    }

    public ValueTask<Gp5PageSetup> ReadPageSetupAsync() =>
        _reader.ReadPageSetupAsync();

    public ValueTask<Gp5Tempo> ReadHeaderTempoAsync() =>
        _reader.ReadHeaderTempoAsync();

    public async ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync()
    {
        var keySignature = await _reader.ReadHeaderKeySignatureAsync();

        if (keySignature.Octave != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"expected to be 0: Octave={keySignature.Octave}");

        if (keySignature.Key >= 0)
        {
            if (keySignature._A01 != 0 || keySignature._A02 != 0 || keySignature._A03 != 0)
                // TODO: message
                throw new FileSerializationIntegrityException($"expected to be 0: _A01={keySignature._A01}, _A02={keySignature._A02}, _A03={keySignature._A03}, ");
        }
        else
        {
            if (keySignature._A01 != -1 || keySignature._A02 != -1 || keySignature._A03 != -1)
                // TODO: message
                throw new FileSerializationIntegrityException($"expected to be -1: _A01={keySignature._A01}, _A02={keySignature._A02}, _A03={keySignature._A03}, ");
        }

        return keySignature;
    }

    public async ValueTask<Gp5MidiChannel> ReadMidiChannelAsync()
    {
        var midiChannel = await _reader.ReadMidiChannelAsync();

        if (midiChannel._A01 != 0 || midiChannel._A02 != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"midiChannel _A01,_A02 expected to be 0: _A01={midiChannel._A01}, _A02={midiChannel._A02}");

        return midiChannel;
    }

    public ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync() =>
        _reader.ReadMusicalDirectionsAsync();

    public ValueTask<int> ReadRseMasterEffectReverbAsync() =>
        _reader.ReadRseMasterEffectReverbAsync();

    public async ValueTask<(int measuresCount, int tracksCount)> ReadMeasuresAndTracksCountAsync()
    {
        var (measuresCount, tracksCount) = await _reader.ReadMeasuresAndTracksCountAsync();

        if (measuresCount < 1 || measuresCount > 2048)
            // TODO: message
            throw new FileSerializationIntegrityException($"measuresCount out of valid range: measuresCount={measuresCount}");

        if (tracksCount < 1 || tracksCount > 127)
            // TODO: message
            throw new FileSerializationIntegrityException($"tracksCount out of valid range: tracksCount={tracksCount}");

        return (measuresCount, tracksCount);
    }

    public async ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst)
    {
        var measureHeader = await _reader.ReadMeasureHeaderAsync(isFirst);

        if (measureHeader._A01 != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"measure header _A01 expected to be 0: _A01={measureHeader._A01}");

        if (!measureHeader.PrimaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasAlternateEndings) && measureHeader.AlternateEndingsFlags != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"AlternateEndingsFlags expected to be 0 due to measure has no alternate endings: AlternateEndingsFlags={measureHeader.AlternateEndingsFlags}");

        return measureHeader;
    }

    public async ValueTask<Gp5Track> ReadTrackAsync()
    {
        var track = await _reader.ReadTrackAsync();

        if (!track.PrimaryFlags.HasFlag(Gp5Track.Primary._A01))
            // TODO: message
            throw new FileSerializationIntegrityException("expected Primary._A01");

        if (track.PrimaryFlags.HasFlag(Gp5Track.Primary.TrackTypePercussion) ==
            track.SecondaryFlags.HasFlag(Gp5Track.Secondary.TrackTypeInstrument))
            // TODO: message
            throw new FileSerializationIntegrityException($"expected to have consistent track type (instrument or percussion): instrument:{track.SecondaryFlags.HasFlag(Gp5Track.Secondary.TrackTypeInstrument)}, percussion:{track.PrimaryFlags.HasFlag(Gp5Track.Primary.TrackTypePercussion)}");

        if (track.StringsCount >= 6 && (track._A01 != 0 || track._A02 != 0))
        {
            // TODO: message
            throw new FileSerializationIntegrityException($"A1/2 expected to be 0 ({track.StringsCount} strings): _A01={track._A01}, _A02={track._A02}");
        }
        else if (track.StringsCount < 6 && (track._A01 != 12 || track._A02 != 12))
        {
            // TODO: message
            throw new FileSerializationIntegrityException($"A1/2 expected to be 12 ({track.StringsCount} strings): _A01={track._A01}, _A02={track._A02}");
        }

        if (track._A03 != 100)
            // TODO: message
            throw new FileSerializationIntegrityException($"A3 expected to be 100: _A03={track._A03}");

        var decodedBValue = track._B01 * 2 + track._B02 * 125 + track._B03 * 384 + track._B04 * 1 + track._B05 * 1 + track._B06 * 22 + track._B07 * -20 + track._B08 * 100 + track._B09 * -98 + track._B10 * -30;
        if (decodedBValue != track._C01)
            // TODO: message
            throw new FileSerializationIntegrityException($"B expected to be valid sequence: sequence={string.Join(",", track._B01, track._B02, track._B03, track._B04, track._B05, track._B06, track._B07, track._B08, track._B09, track._B10)}, _C01={track._C01}, decodedBValue={decodedBValue}");

        if (track.SecondaryFlags.HasFlag(Gp5Track.Secondary.DisplayTablature) &&
            track.SecondaryFlags.HasFlag(Gp5Track.Secondary.DisplayStandardNotation) &&
            track._C01 != 1023)
        {
            // TODO: message
            throw new FileSerializationIntegrityException($"C expected to be 1023: _C01={track._C01}");
        }
        else if (track.SecondaryFlags.HasFlag(Gp5Track.Secondary.DisplayTablature) &&
            !track.SecondaryFlags.HasFlag(Gp5Track.Secondary.DisplayStandardNotation) &&
            track._C01 != 991)
        {
            // TODO: message
            throw new FileSerializationIntegrityException($"C expected to be 991: _C01={track._C01}");
        }
        else if (!track.SecondaryFlags.HasFlag(Gp5Track.Secondary.DisplayTablature) &&
            track.SecondaryFlags.HasFlag(Gp5Track.Secondary.DisplayStandardNotation) &&
            track._C01 != 767)
        {
            // TODO: message
            throw new FileSerializationIntegrityException($"C expected to be 767: _C01={track._C01}");
        }

        if (track._E01 != -1)
            // TODO: message
            throw new FileSerializationIntegrityException($"E expected to be -1: _E01={track._E01}");

        if (track.RseInstrument == -1)
        {
            if (track.PrimaryFlags.HasFlag(Gp5Track.Primary.UseRse) ||
                track.RseSoundBank != -1 || track._D01 != -1 ||
                track.RseEffectName.Length != 0 || track.RseEffectCategoryName.Length != 0)
                // TODO: message
                throw new FileSerializationIntegrityException($"RSE props expected to be -1: Instrument={track.RseInstrument}, RseSoundBank={track.RseSoundBank}, _D01={track._D01}, RseEffectName={track.RseEffectName}, RseEffectCategoryName={track.RseEffectCategoryName},");

            return track;
        }

        if (track.RseInstrument == 128)
        {
            if (track._D01 != 0)
                // TODO: message
                throw new FileSerializationIntegrityException($"_D01 expected to be 0: Instrument={track.RseInstrument}, _D01={track._D01},");

            return track;
        }

        if (track.RseInstrument == 24 || track.RseInstrument == 25 || track.RseInstrument == 26 || track.RseInstrument == 27)
        {
            if (track._D01 != 1)
                // TODO: message
                throw new FileSerializationIntegrityException($"_D01 expected to be 1: Instrument={track.RseInstrument}, _D01={track._D01},");

            return track;
        }

        if (track.RseInstrument == 33 || track.RseInstrument == 34 || track.RseInstrument == 36)
        {
            if (track._D01 != 2)
                // TODO: message
                throw new FileSerializationIntegrityException($"_D01 expected to be 2: Instrument={track.RseInstrument}, _D01={track._D01},");

            return track;
        }

        // TODO: message
        throw new FileSerializationIntegrityException($"Instrument expected to be -1,128,24,25,26,27,33,34,36: Instrument={track.RseInstrument}, _D01={track._D01},");
    }

    public ValueTask<byte> ReadMeasureBreakLineAsync() =>
        _reader.ReadMeasureBreakLineAsync();

    public async ValueTask<int> ReadMeasureBeatsCountAsync()
    {
        var beatsCount = await _reader.ReadMeasureBeatsCountAsync();

        if (beatsCount < 1 || beatsCount > 127)
            // TODO: message
            throw new FileSerializationIntegrityException($"beatsCount out of valid range: beatsCount={beatsCount}");

        return beatsCount;
    }

    public ValueTask<Gp5Beat> ReadBeatAsync() =>
        _reader.ReadBeatAsync();
}
