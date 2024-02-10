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
        await ProcessAsync();
        PrintDeserializedFileJson();
        return new Gp5Score();
    }

    [Obsolete("Temporary runtime testing")]
    private void PrintDeserializedFileJson() =>
        Console.WriteLine(JsonSerializer.Serialize(File, new JsonSerializerOptions { WriteIndented = true }));

    protected override async ValueTask NextVersionAsync() =>
        File.Version = await _compositeTypesDecoder.ReadVersionAsync();

    protected override async ValueTask NextScoreInformationAsync() =>
        File.ScoreInformation = await _compositeTypesDecoder.ReadScoreInformationAsync();

    protected override async ValueTask NextLyricsAsync() =>
        File.Lyrics = await _compositeTypesDecoder.ReadLyricsAsync();

    protected override async ValueTask NextRseMasterEffectAsync() =>
        File.RseMasterEffect = await _compositeTypesDecoder.ReadRseMasterEffectAsync();

    protected override async ValueTask NextPageSetupAsync() =>
        File.PageSetup = await _compositeTypesDecoder.ReadPageSetupAsync();

    protected override async ValueTask NextHeaderTempoAsync() =>
        File.Tempo = await _compositeTypesDecoder.ReadHeaderTempoAsync();

    protected override async ValueTask NextHeaderKeySignatureAsync() =>
        File.KeySignature = await _compositeTypesDecoder.ReadHeaderKeySignatureAsync();

    protected sealed override ValueTask NextMidiChannelsAsync()
    {
        const int midiChannelsCount = 64;
        File.MidiChannels = new Gp5MidiChannel[midiChannelsCount];
        return base.NextMidiChannelsAsync();
    }

    protected override async ValueTask NextMidiChannelAsync(int index) =>
        File.MidiChannels[index] = await _compositeTypesDecoder.ReadMidiChannelAsync();

    protected override async ValueTask NextMusicalDirectionsAsync() =>
        File.MusicalDirections = await _compositeTypesDecoder.ReadMusicalDirectionsAsync();

    protected override async ValueTask NextRseMasterEffectReverbAsync() =>
        File.RseMasterEffect.Reverb = await _primitivesDecoder.ReadIntAsync();

    protected override async ValueTask NextMeasuresCountAsync() =>
        File.MeasuresCount = await _primitivesDecoder.ReadIntAsync();

    protected override async ValueTask NextTracksCountAsync() =>
        File.TracksCount = await _primitivesDecoder.ReadIntAsync();

    protected sealed override ValueTask NextMeasureHeadersAsync()
    {
        File.MeasureHeaders = new Gp5MeasureHeader[File.MeasuresCount];
        return base.NextMeasureHeadersAsync();
    }

    protected override async ValueTask NextMeasureHeaderAsync(int index) =>
        File.MeasureHeaders[index] = await _compositeTypesDecoder.ReadMeasureHeaderAsync(isFirst: index == 0);
}
