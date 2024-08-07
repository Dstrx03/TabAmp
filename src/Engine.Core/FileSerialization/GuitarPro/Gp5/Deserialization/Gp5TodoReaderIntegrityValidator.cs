﻿using System.Threading.Tasks;
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

    public ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync() =>
        _reader.ReadRseMasterEffectAsync();

    public ValueTask<Gp5PageSetup> ReadPageSetupAsync() =>
        _reader.ReadPageSetupAsync();

    public ValueTask<Gp5Tempo> ReadHeaderTempoAsync() =>
        _reader.ReadHeaderTempoAsync();

    public ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync() =>
        _reader.ReadHeaderKeySignatureAsync();

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

        if (track._E01 != -1)
            // TODO: message
            throw new FileSerializationIntegrityException($"E expected to be -1: _E01={track._E01}");

        return track;
    }
}
