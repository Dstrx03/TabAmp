using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Tracks;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.IntegrityValidators;

internal class Gp5TracksReaderIntegrityValidator : IGp5TracksReader
{
    private readonly IGp5TracksReader _tracksReader;

    public Gp5TracksReaderIntegrityValidator(IGp5TracksReader tracksReader) =>
        _tracksReader = tracksReader;

    public async ValueTask<Gp5MidiChannel> ReadMidiChannelAsync()
    {
        var midiChannel = await _tracksReader.ReadMidiChannelAsync();

        if (midiChannel._A01 != 0 || midiChannel._A02 != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"midiChannel _A01,_A02 expected to be 0: _A01={midiChannel._A01}, _A02={midiChannel._A02}");

        return midiChannel;
    }

    public async ValueTask<Gp5Track> ReadTrackAsync()
    {
        var track = await _tracksReader.ReadTrackAsync();

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

        return track;

        /*
        // TODO: moved to ReadRseInstrumentAsync() and ReadRseInstrumentEffectAsync()
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
        */
    }

    public ValueTask<Gp5MixTable> ReadMixTableAsync() =>
        _tracksReader.ReadMixTableAsync();

    public async ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync()
    {
        var rseMasterEffect = await _tracksReader.ReadRseMasterEffectAsync();

        if (rseMasterEffect._A01 != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"expected to be 0: _A01={rseMasterEffect._A01}");

        return rseMasterEffect;
    }

    public ValueTask<int> ReadRseMasterEffectReverbAsync() =>
        _tracksReader.ReadRseMasterEffectReverbAsync();
}
