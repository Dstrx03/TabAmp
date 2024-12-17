using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

[Obsolete]
internal class Gp5TodoReader : IGp5TodoReader
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;
    private readonly IGp5TextReader _textReader;
    private readonly IGp5RseEqualizerReader _rseEqualizerReader;

    public Gp5TodoReader(IGp5BinaryPrimitivesReader primitivesReader, IGp5TextReader textReader,
        IGp5RseEqualizerReader rseEqualizerReader)
    {
        _primitivesReader = primitivesReader;
        _textReader = textReader;
        _rseEqualizerReader = rseEqualizerReader;
    }

    

    

    

    

    

    

    public async ValueTask<Gp5Tempo> ReadHeaderTempoAsync()//TODO: move to deserializer
    {
        var tempo = await ReadTempoAsync();
        tempo.HideBeatsPerMinute = await _primitivesReader.ReadBoolAsync();// TODO: named method via Demetra principle

        return tempo;
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
            measureHeader.TimeSignature = await ReadTimeSignatureAsync(hasDenominator);

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasRepeatClose))
            measureHeader.RepeatsCount = await _primitivesReader.ReadByteAsync();

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasMarker))
            measureHeader.Marker = await ReadMarkerAsync();

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasKeySignature))
            measureHeader.KeySignature = await ReadKeySignatureAsync();

        var hasAlternateEndings = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasAlternateEndings);
        if (isFirst)
        {
            if (hasTimeSignature)
                measureHeader.TimeSignature!.BeamGroups = await ReadTimeSignatureBeamGroupsAsync();

            if (hasAlternateEndings)
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesReader.ReadByteAsync();
        }
        else
        {
            if (hasAlternateEndings)
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesReader.ReadByteAsync();

            if (hasTimeSignature)
                measureHeader.TimeSignature!.BeamGroups = await ReadTimeSignatureBeamGroupsAsync();
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
            beat.Chord = await ReadChordAsync();

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasText))
            beat.Text = await _textReader.ReadIntByteTextAsync();

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasEffects))
            beat.Effects = await ReadBeatEffectsAsync();

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasMixTable))
            beat.MixTable = await ReadMixTableAsync();

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

        // TODO: move to the integrity validation layer
        if (!primaryFlags.HasFlag(Gp5Note.Primary._A01))
            throw new FileSerializationIntegrityException("note expected to have primary flag _A01");

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
            note.Effects = await ReadNoteEffectsAsync();

        return note;
    }
}
