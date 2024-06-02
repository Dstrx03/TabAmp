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

    public ValueTask<Gp5MidiChannel> ReadMidiChannelAsync() =>
        _reader.ReadMidiChannelAsync();

    public ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync() =>
        _reader.ReadMusicalDirectionsAsync();

    public ValueTask<int> ReadRseMasterEffectReverbAsync() =>
        _reader.ReadRseMasterEffectReverbAsync();

    public async ValueTask<int> ReadMeasuresCountAsync()
    {
        var measuresCount = await _reader.ReadMeasuresCountAsync();

        if (measuresCount < 1 || measuresCount > 2048)
            // TODO: message
            throw new FileSerializationIntegrityException($"measuresCount out of valid range: measuresCount={measuresCount}");

        return measuresCount;
    }

    public async ValueTask<int> ReadTracksCountAsync()
    {
        var tracksCount = await _reader.ReadTracksCountAsync();

        if(tracksCount < 1 || tracksCount > 127)
            // TODO: message
            throw new FileSerializationIntegrityException($"tracksCount out of valid range: tracksCount={tracksCount}");

        return tracksCount;
    }

    public ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst) =>
        _reader.ReadMeasureHeaderAsync(isFirst);
}
