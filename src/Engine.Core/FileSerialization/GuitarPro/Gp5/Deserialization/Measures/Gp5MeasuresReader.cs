using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.BinaryPrimitives;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.DocumentComponents;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.MusicalNotation;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Text;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Tracks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Measures;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Measures;

internal class Gp5MeasuresReader : IGp5MeasuresReader
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;
    private readonly IGp5TextReader _textReader;
    private readonly IGp5DocumentComponentsReader _documentReader;
    private readonly IGp5MusicalNotationReader _notationReader;
    private readonly IGp5TracksReader _tracksReader;

    public Gp5MeasuresReader(IGp5BinaryPrimitivesReader primitivesReader,
        IGp5TextReader textReader,
        IGp5DocumentComponentsReader documentReader,
        IGp5MusicalNotationReader notationReader,
        IGp5TracksReader tracksReader)
    {
        _primitivesReader = primitivesReader;
        _textReader = textReader;
        _documentReader = documentReader;
        _notationReader = notationReader;
        _tracksReader = tracksReader;
    }

    public async ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst)
    {
        var primaryFlags = (Gp5MeasureHeader.Primary)await _primitivesReader.ReadByteAsync();
        var measureHeader = new Gp5MeasureHeader
        {
            PrimaryFlags = primaryFlags
        };

        var hasTimeSignature = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignature);
        var hasDenominator = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignatureDenominator);

        if (hasTimeSignature)
            measureHeader.TimeSignature = await _notationReader.ReadTimeSignatureAsync(hasDenominator);

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasRepeatClose))
            measureHeader.RepeatsCount = await _primitivesReader.ReadByteAsync();

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasMarker))
            measureHeader.Marker = await _documentReader.ReadMarkerAsync();

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasKeySignature))
            measureHeader.KeySignature = await _notationReader.ReadKeySignatureAsync();

        var hasAlternateEndings = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasAlternateEndings);
        if (isFirst)
        {
            if (hasTimeSignature)
                measureHeader.TimeSignature!.BeamGroups = await _notationReader.ReadTimeSignatureBeamGroupsAsync();

            if (hasAlternateEndings)
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesReader.ReadByteAsync();
        }
        else
        {
            if (hasAlternateEndings)
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesReader.ReadByteAsync();

            if (hasTimeSignature)
                measureHeader.TimeSignature!.BeamGroups = await _notationReader.ReadTimeSignatureBeamGroupsAsync();
        }

        if (!hasAlternateEndings)
            measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesReader.ReadByteAsync();

        measureHeader.TripletFeel = await _primitivesReader.ReadByteAsync();
        measureHeader._A01 = await _primitivesReader.ReadByteAsync();

        return measureHeader;
    }

    public ValueTask<byte> ReadMeasureBreakLineAsync() =>
        _primitivesReader.ReadByteAsync();

    public ValueTask<int> ReadMeasureBeatsCountAsync() =>
        _primitivesReader.ReadIntAsync();

    public async ValueTask<Gp5Beat> ReadBeatAsync(Func<Gp5Beat, ValueTask> readNotesAsync)
    {
        var primaryFlags = (Gp5Beat.Primary)await _primitivesReader.ReadByteAsync();
        var beat = new Gp5Beat
        {
            PrimaryFlags = primaryFlags,
        };

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasStatus))
            beat.Status = await _primitivesReader.ReadByteAsync();

        beat.Duration = await _primitivesReader.ReadSignedByteAsync();

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasTuplet))
            beat.Tuplet = await _primitivesReader.ReadIntAsync();

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasChord))
            beat.Chord = await _notationReader.ReadChordAsync();

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasText))
            beat.Text = await _textReader.ReadIntByteTextAsync();

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasEffects))
            beat.Effects = await _notationReader.ReadBeatEffectsAsync();

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasMixTable))
            beat.MixTable = await _tracksReader.ReadMixTableAsync();

        beat.NotesPresenceFlags = (Gp5Beat.NotesPresence)await _primitivesReader.ReadByteAsync();

        await readNotesAsync(beat);

        // throw new NotImplementedException("TODO: complete beat reading, test flags.");

        beat.SecondaryFlags = (Gp5Beat.Secondary)await _primitivesReader.ReadShortAsync();

        if (beat.SecondaryFlags.HasFlag(Gp5Beat.Secondary.BreakSecondary_TODO))
        {
            beat.TODO = await _primitivesReader.ReadByteAsync();
            throw new NotImplementedException($"TODO: research unknown flag and data, value={beat.TODO}");
        }

        return beat;
    }

    public async ValueTask<Gp5Note> ReadNoteAsync()
    {
        var primaryFlags = (Gp5Note.Primary)await _primitivesReader.ReadByteAsync();
        var note = new Gp5Note
        {
            PrimaryFlags = primaryFlags,
            Type = await _primitivesReader.ReadByteAsync()
        };

        if (primaryFlags.HasFlag(Gp5Note.Primary.HasDynamic))
            note.Dynamic = await _primitivesReader.ReadByteAsync();

        note.Fret = await _primitivesReader.ReadByteAsync();

        if (primaryFlags.HasFlag(Gp5Note.Primary.HasFingering))
        {
            note.LeftHandFingering = await _primitivesReader.ReadSignedByteAsync();
            note.RightHandFingering = await _primitivesReader.ReadSignedByteAsync();
        }

        if (primaryFlags.HasFlag(Gp5Note.Primary.HasSoundDuration))
            note.SoundDuration = await _primitivesReader.ReadDoubleAsync();

        note.SecondaryFlags = (Gp5Note.Secondary)await _primitivesReader.ReadByteAsync();

        if (primaryFlags.HasFlag(Gp5Note.Primary.HasEffects))
            note.Effects = await _notationReader.ReadNoteEffectsAsync();

        return note;
    }
}
