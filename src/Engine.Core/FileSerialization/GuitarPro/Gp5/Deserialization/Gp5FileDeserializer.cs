using System;
using System.Text.Json;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Measures;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Tracks;
using TabAmp.Engine.Core.Score;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5FileDeserializer : Gp5FileSerializationProcessor, IFileDeserializer<Gp5Score>
{
    private readonly IGp5DocumentComponentsReader _documentReader;
    private readonly IGp5MusicalNotationReader _notationReader;
    private readonly IGp5TracksReader _tracksReader;
    private readonly IGp5MeasuresReader _measuresReader;

    public Gp5FileDeserializer(IGp5DocumentComponentsReader documentReader, IGp5MusicalNotationReader notationReader,
        IGp5TracksReader tracksReader, IGp5MeasuresReader measuresReader) =>
        (_documentReader, _notationReader, _tracksReader, _measuresReader) =
        (documentReader, notationReader, tracksReader, measuresReader);

    public async Task<Gp5Score> DeserializeAsync()
    {
        await ProcessAsync(new());
        PrintDeserializedFileJson();
        return new Gp5Score();
    }

    [Obsolete("Temporary runtime testing")]
    private void PrintDeserializedFileJson()
    {
        Console.WriteLine(JsonSerializer.Serialize(File, new JsonSerializerOptions { WriteIndented = true }));
    }

    protected override async ValueTask NextVersionAsync() =>
        File.Version = await _documentReader.ReadVersionAsync();

    protected override async ValueTask NextScoreInformationAsync() =>
        File.ScoreInformation = await _documentReader.ReadScoreInformationAsync();

    protected override async ValueTask NextLyricsAsync() =>
        File.Lyrics = await _documentReader.ReadLyricsAsync();

    protected override async ValueTask NextRseMasterEffectAsync() =>
        File.RseMasterEffect = await _tracksReader.ReadRseMasterEffectAsync();

    protected override async ValueTask NextPageSetupAsync() =>
        File.PageSetup = await _documentReader.ReadPageSetupAsync();

    protected override async ValueTask NextHeaderTempoAsync()
    {
        File.Tempo = await _notationReader.ReadTempoAsync();
        File.Tempo.HideBeatsPerMinute = await _notationReader.ReadTempoHideBeatsPerMinuteAsync();
    }

    protected override async ValueTask NextHeaderKeySignatureAsync() =>
        File.KeySignature = await _notationReader.ReadHeaderKeySignatureAsync();

    protected override ValueTask NextMidiChannelsAsync()
    {
        File.MidiChannels = new Gp5MidiChannel[Gp5File.MidiChannelsLength];
        return base.NextMidiChannelsAsync();
    }

    protected override async ValueTask NextMidiChannelAsync(int index) =>
        File.MidiChannels[index] = await _tracksReader.ReadMidiChannelAsync();

    protected override async ValueTask NextMusicalDirectionsAsync() =>
        File.MusicalDirections = await _notationReader.ReadMusicalDirectionsAsync();

    protected override async ValueTask NextRseMasterEffectReverbAsync() =>
        File.RseMasterEffect.Reverb = await _tracksReader.ReadRseMasterEffectReverbAsync();

    protected override async ValueTask NextMeasureHeadersAndTracksCountAsync()
    {
        var (measureHeadersCount, tracksCount) = await _documentReader.ReadMeasureHeadersAndTracksCountAsync();
        File.MeasureHeaders = new Gp5MeasureHeader[measureHeadersCount];
        File.Tracks = new Gp5Track[tracksCount];

        var measuresCount = measureHeadersCount * tracksCount;
        File.MeasureBreakLines = new byte[measuresCount];
        File.MeasureBeats = new Gp5Beat[measuresCount * 2][];
    }

    protected override async ValueTask NextMeasureHeaderAsync(int index) =>
        File.MeasureHeaders[index] = await _measuresReader.ReadMeasureHeaderAsync(isFirst: index == 0);

    protected override async ValueTask NextTrackAsync(int index) =>
        File.Tracks[index] = await _tracksReader.ReadTrackAsync();

    protected override async ValueTask NextMeasureBreakLineAsync(int measureIndex) =>
        File.MeasureBreakLines[measureIndex] = await _measuresReader.ReadMeasureBreakLineAsync();

    protected override async ValueTask NextMeasureBeatsCountAsync(int measureIndex) =>
        File.MeasureBeats[measureIndex] = new Gp5Beat[await _measuresReader.ReadMeasureBeatsCountAsync()];

    protected override async ValueTask NextBeatAsync(int measureIndex, int beatIndex) =>
        File.MeasureBeats[measureIndex][beatIndex] = await _measuresReader.ReadBeatAsync(NextNotesAsync);

    protected override ValueTask NextNotesAsync(Gp5Beat beat)
    {
        var notesCount = CalculateNotesCount(beat.NotesPresenceFlags);
        if (notesCount == 0)
            return ValueTask.CompletedTask;

        beat.Notes = new Gp5Note[notesCount];
        return base.NextNotesAsync(beat);
    }

    private int CalculateNotesCount(Gp5Beat.NotesPresence notesPresenceFlags) =>
        Convert.ToByte(notesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasFirstStringNote)) +
        Convert.ToByte(notesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasSecondStringNote)) +
        Convert.ToByte(notesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasThirdStringNote)) +
        Convert.ToByte(notesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasFourthStringNote)) +
        Convert.ToByte(notesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasFifthStringNote)) +
        Convert.ToByte(notesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasSixthStringNote)) +
        Convert.ToByte(notesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasSeventhStringNote));

    protected override async ValueTask NextNoteAsync(Gp5Note[] notes, int index) =>
        notes[index] = await _measuresReader.ReadNoteAsync();
}
