using System;
using System.Text.Json;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.Score;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5FileDeserializer : Gp5FileSerializationProcessor, IFileDeserializer<Gp5Score>
{
    private readonly IGp5TodoReader _reader;

    public Gp5FileDeserializer(IGp5TodoReader reader) =>
        _reader = reader;

    public async Task<Gp5Score> DeserializeAsync()
    {
        await ProcessAsync();
        PrintDeserializedFileJson();
        return new Gp5Score();
    }

    [Obsolete("Temporary runtime testing")]
    private void PrintDeserializedFileJson()
    {
        Console.WriteLine(JsonSerializer.Serialize(File, new JsonSerializerOptions { WriteIndented = true }));
    }

    protected override async ValueTask NextVersionAsync() =>
        File.Version = await _reader.ReadVersionAsync();

    protected override async ValueTask NextScoreInformationAsync() =>
        File.ScoreInformation = await _reader.ReadScoreInformationAsync();

    protected override async ValueTask NextLyricsAsync() =>
        File.Lyrics = await _reader.ReadLyricsAsync();

    protected override async ValueTask NextRseMasterEffectAsync() =>
        File.RseMasterEffect = await _reader.ReadRseMasterEffectAsync();

    protected override async ValueTask NextPageSetupAsync() =>
        File.PageSetup = await _reader.ReadPageSetupAsync();

    protected override async ValueTask NextHeaderTempoAsync() =>
        File.Tempo = await _reader.ReadHeaderTempoAsync();

    protected override async ValueTask NextHeaderKeySignatureAsync() =>
        File.KeySignature = await _reader.ReadHeaderKeySignatureAsync();

    protected override ValueTask NextMidiChannelsAsync()
    {
        File.MidiChannels = new Gp5MidiChannel[Gp5File.MidiChannelsLength];
        return base.NextMidiChannelsAsync();
    }

    protected override async ValueTask NextMidiChannelAsync(int index) =>
        File.MidiChannels[index] = await _reader.ReadMidiChannelAsync();

    protected override async ValueTask NextMusicalDirectionsAsync() =>
        File.MusicalDirections = await _reader.ReadMusicalDirectionsAsync();

    protected override async ValueTask NextRseMasterEffectReverbAsync() =>
        File.RseMasterEffect.Reverb = await _reader.ReadRseMasterEffectReverbAsync();

    protected override async ValueTask NextMeasuresAndTracksCountAsync()
    {
        var (measuresCount, tracksCount) = await _reader.ReadMeasuresAndTracksCountAsync();

        File.MeasureHeaders = new Gp5MeasureHeader[measuresCount];
        File.Tracks = new Gp5Track[tracksCount];
    }

    protected override async ValueTask NextMeasureHeaderAsync(int index) =>
        File.MeasureHeaders[index] = await _reader.ReadMeasureHeaderAsync(isFirst: index == 0);

    protected override async ValueTask NextTrackAsync(int index) =>
        File.Tracks[index] = await _reader.ReadTrackAsync();

    protected override ValueTask NextMeasuresAsync()
    {
        File.Beats = new Gp5Beat[/*File.MeasureHeaders.Length * File.Tracks.Length*/1][];
        return base.NextMeasuresAsync();
    }

    protected override async ValueTask NextMeasureBeatsCountAsync(int measureIndex)
    {
        var beatsCount = await _reader.ReadMeasureBeatsCountAsync();
        if (beatsCount == 0)
            return;

        File.Beats[measureIndex] = new Gp5Beat[beatsCount];
    }

    protected override async ValueTask NextBeatAsync(int beatIndex, int measureIndex)
    {
        File.Beats[measureIndex][beatIndex] = null; // TODO: reading
    }
}
