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
            throw new FileSerializationIntegrityException($"AlternateEndingsFlags expected to be 0 due to measure has no laternate endings: AlternateEndingsFlags={measureHeader.AlternateEndingsFlags}");

        return measureHeader;
    }
}
