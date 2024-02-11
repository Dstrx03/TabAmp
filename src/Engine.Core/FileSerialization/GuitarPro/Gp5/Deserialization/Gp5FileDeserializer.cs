using System;
using System.Text.Json;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.Score;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal sealed class Gp5FileDeserializer : Gp5FileSerializationProcessor, IFileDeserializer<Gp5Score>
{
    private readonly Gp5CompositeTypesDeserializer _deserializer;

    public Gp5FileDeserializer(Gp5CompositeTypesDeserializer deserializer) =>
        _deserializer = deserializer;

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
        File.Version = await _deserializer.ReadVersionAsync();

    protected override async ValueTask NextScoreInformationAsync() =>
        File.ScoreInformation = await _deserializer.ReadScoreInformationAsync();

    protected override async ValueTask NextLyricsAsync() =>
        File.Lyrics = await _deserializer.ReadLyricsAsync();

    protected override async ValueTask NextRseMasterEffectAsync() =>
        File.RseMasterEffect = await _deserializer.ReadRseMasterEffectAsync();

    protected override async ValueTask NextPageSetupAsync() =>
        File.PageSetup = await _deserializer.ReadPageSetupAsync();

    protected override async ValueTask NextHeaderTempoAsync() =>
        File.Tempo = await _deserializer.ReadHeaderTempoAsync();

    protected override async ValueTask NextHeaderKeySignatureAsync() =>
        File.KeySignature = await _deserializer.ReadHeaderKeySignatureAsync();

    protected sealed override ValueTask NextMidiChannelsAsync()
    {
        File.MidiChannels = new Gp5MidiChannel[Gp5File.MidiChannelsCount];
        return base.NextMidiChannelsAsync();
    }

    protected override async ValueTask NextMidiChannelAsync(int index) =>
        File.MidiChannels[index] = await _deserializer.ReadMidiChannelAsync();

    protected override async ValueTask NextMusicalDirectionsAsync() =>
        File.MusicalDirections = await _deserializer.ReadMusicalDirectionsAsync();

    protected override async ValueTask NextRseMasterEffectReverbAsync() =>
        File.RseMasterEffect.Reverb = await _deserializer.ReadRseMasterEffectReverbAsync();

    protected override async ValueTask NextMeasuresCountAsync() =>
        File.MeasuresCount = await _deserializer.ReadMeasuresCountAsync();

    protected override async ValueTask NextTracksCountAsync() =>
        File.TracksCount = await _deserializer.ReadTracksCountAsync();

    protected sealed override ValueTask NextMeasureHeadersAsync()
    {
        File.MeasureHeaders = new Gp5MeasureHeader[File.MeasuresCount];
        return base.NextMeasureHeadersAsync();
    }

    protected override async ValueTask NextMeasureHeaderAsync(int index) =>
        File.MeasureHeaders[index] = await _deserializer.ReadMeasureHeaderAsync(isFirst: index == 0);
}
