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

        if (measureHeader.EndOfObjectSeparator != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"EndOfObjectSeparator expected to be 0: EndOfObjectSeparator={measureHeader.EndOfObjectSeparator}");

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

        if (track.PrimaryFlags.HasFlag(Gp5Track.Primary.PercussionTrack) ==
            track.SecondaryFlags.HasFlag(Gp5Track.Secondary.InstrumentTrack))
            // TODO: message
            throw new FileSerializationIntegrityException($"expected to have consistent track type: instrument:{track.SecondaryFlags.HasFlag(Gp5Track.Secondary.InstrumentTrack)}, percussion:{track.PrimaryFlags.HasFlag(Gp5Track.Primary.PercussionTrack)}");

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

        // TODO: figure out validation for B & C
        /*
        if (track._B01 != 1 ||
            track._B02 != 2 ||
            track._B03 != 3 ||
            track._B04 != 4 ||
            track._B05 != 5 ||
            track._B06 != 6 ||
            track._B07 != 7 ||
            track._B08 != 8 ||
            track._B09 != 9 ||
            track._B10 != 10)
            // TODO: message
            throw new FileSerializationIntegrityException($"B expected to be 1,2,3,4,5,6,7,8,9,10: sequence={string.Join(",", track._B01, track._B02, track._B03, track._B04, track._B05, track._B06, track._B07, track._B08, track._B09, track._B10)}");

        if (track._C01 != 1023)
            // TODO: message
            throw new FileSerializationIntegrityException($"C expected to be 1023: _C01={track._C01}");
        */

        if (track._E01 != -1)
            // TODO: message
            throw new FileSerializationIntegrityException($"E expected to be -1: _E01={track._E01}");

        if (track.Instrument == -1)
        {
            if (track.PrimaryFlags.HasFlag(Gp5Track.Primary.UseRse) ||
                track.RseSoundBank != -1 || track._D01 != -1 ||
                track.RseEffectName.Length != 0 || track.RseEffectCategoryName.Length != 0)
                // TODO: message
                throw new FileSerializationIntegrityException($"RSE props expected to be -1: Instrument={track.Instrument}, RseSoundBank={track.RseSoundBank}, _D01={track._D01}, RseEffectName={track.RseEffectName}, RseEffectCategoryName={track.RseEffectCategoryName},");

            return track;
        }

        if (track.Instrument == 128)
        {
            if (track._D01 != 0)
                // TODO: message
                throw new FileSerializationIntegrityException($"_D01 expected to be 0: Instrument={track.Instrument}, _D01={track._D01},");

            return track;
        }

        if (track.Instrument == 24 || track.Instrument == 25 || track.Instrument == 26 || track.Instrument == 27)
        {
            if (track._D01 != 1)
                // TODO: message
                throw new FileSerializationIntegrityException($"_D01 expected to be 1: Instrument={track.Instrument}, _D01={track._D01},");

            return track;
        }

        if (track.Instrument == 33 || track.Instrument == 34 || track.Instrument == 36)
        {
            if (track._D01 != 2)
                // TODO: message
                throw new FileSerializationIntegrityException($"_D01 expected to be 2: Instrument={track.Instrument}, _D01={track._D01},");

            return track;
        }

        // TODO: message
        throw new FileSerializationIntegrityException($"Instrument expected to be -1,128,24,25,26,27,33,34,36: Instrument={track.Instrument}, _D01={track._D01},");
    }
}
