using System;
using System.Text.Json;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.Score;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal sealed class Gp5FileDeserializer : Gp5FileSerializationProcessor, IFileDeserializer<Gp5Score>
{
    private readonly Gp5PrimitivesSerialDecoder _primitivesDecoder;
    private readonly Gp5CompositeTypesSerialDecoder _compositeTypesDecoder;

    public Gp5FileDeserializer(Gp5PrimitivesSerialDecoder primitivesDecoder, Gp5CompositeTypesSerialDecoder compositeTypesDecoder)
    {
        _primitivesDecoder = primitivesDecoder;
        _compositeTypesDecoder = compositeTypesDecoder;
    }

    public async Task<Gp5Score> DeserializeAsync()
    {
        _file = new Gp5File();
        await ProcessAsync();

        Console.WriteLine(JsonSerializer.Serialize(_file, new JsonSerializerOptions { WriteIndented = true }));

        return new Gp5Score();
    }

    protected override async ValueTask NextVersionAsync() =>
        _file.Version = await _compositeTypesDecoder.ReadVersionAsync();

    protected override async ValueTask NextScoreInformationAsync() =>
        _file.ScoreInformation = await _compositeTypesDecoder.ReadScoreInformationAsync();

    protected override async ValueTask NextLyricsAsync() =>
        _file.Lyrics = await _compositeTypesDecoder.ReadLyricsAsync();

    protected override async ValueTask NextRseMasterEffectAsync() =>
        _file.RseMasterEffect = await _compositeTypesDecoder.ReadRseMasterEffectAsync();

    protected override async ValueTask NextPageSetupAsync() =>
        _file.PageSetup = await _compositeTypesDecoder.ReadPageSetupAsync();

    protected override async ValueTask NextHeaderTempoAsync() =>
        _file.Tempo = await _compositeTypesDecoder.ReadHeaderTempoAsync();

    protected override async ValueTask NextHeaderKeySignatureAsync() =>
        _file.KeySignature = await _compositeTypesDecoder.ReadHeaderKeySignatureAsync();

    protected sealed override ValueTask NextMidiChannelsAsync()
    {
        const int midiChannelsCount = 64;
        _file.MidiChannels = new Gp5MidiChannel[midiChannelsCount];
        return base.NextMidiChannelsAsync();
    }

    protected override async ValueTask NextMidiChannelAsync(int index) =>
        _file.MidiChannels[index] = await _compositeTypesDecoder.ReadMidiChannelAsync();

    protected override async ValueTask NextMusicalDirectionsAsync() =>
        _file.MusicalDirections = await _compositeTypesDecoder.ReadMusicalDirectionsAsync();

    protected override async ValueTask NextRseMasterEffectReverbAsync() =>
        _file.RseMasterEffect.Reverb = await _primitivesDecoder.ReadIntAsync();

    protected override async ValueTask NextMeasuresCountAsync() =>
        _file.MeasuresCount = await _primitivesDecoder.ReadIntAsync();

    protected override async ValueTask NextTracksCountAsync() =>
        _file.TracksCount = await _primitivesDecoder.ReadIntAsync();

    protected sealed override ValueTask NextMeasureHeadersAsync()
    {
        _file.MeasureHeaders = new Gp5MeasureHeader[_file.MeasuresCount];
        return base.NextMeasureHeadersAsync();
    }

    protected override async ValueTask NextMeasureHeaderAsync(int index) =>
        _file.MeasureHeaders[index] = await _compositeTypesDecoder.ReadMeasureHeaderAsync(isFirst: index == 0);
}
