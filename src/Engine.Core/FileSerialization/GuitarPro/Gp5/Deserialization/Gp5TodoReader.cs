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

    

    

    

    

    public async ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync()
    {
        return new Gp5RseMasterEffect
        {
            Volume = await _primitivesReader.ReadIntAsync(),
            _A01 = await _primitivesReader.ReadIntAsync(),
            Equalizer = await _rseEqualizerReader.ReadRseEqualizerAsync(Gp5RseMasterEffect.EqualizerBandsCount)
        };
    }

    

    public async ValueTask<Gp5Tempo> ReadHeaderTempoAsync()//TODO: move to deserializer
    {
        var tempo = await ReadTempoAsync();
        tempo.HideBeatsPerMinute = await _primitivesReader.ReadBoolAsync();// TODO: named method via Demetra principle

        return tempo;
    }

    public async ValueTask<Gp5MidiChannel> ReadMidiChannelAsync()
    {
        return new Gp5MidiChannel
        {
            Instrument = await _primitivesReader.ReadIntAsync(),
            Volume = await _primitivesReader.ReadByteAsync(),
            Balance = await _primitivesReader.ReadByteAsync(),
            Chorus = await _primitivesReader.ReadByteAsync(),
            Reverb = await _primitivesReader.ReadByteAsync(),
            Phaser = await _primitivesReader.ReadByteAsync(),
            Tremolo = await _primitivesReader.ReadByteAsync(),
            _A01 = await _primitivesReader.ReadByteAsync(),
            _A02 = await _primitivesReader.ReadByteAsync()
        };
    }



    public ValueTask<int> ReadRseMasterEffectReverbAsync() =>
        _primitivesReader.ReadIntAsync();

    

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

    public async ValueTask<Gp5Track> ReadTrackAsync()
    {
        return new Gp5Track
        {
            PrimaryFlags = (Gp5Track.Primary)await _primitivesReader.ReadByteAsync(),
            Name = await _textReader.ReadByteTextAsync(Gp5Track.NameMaxLength),
            StringsCount = await _primitivesReader.ReadIntAsync(),
            StringsTuning = await ReadStringsTuningAsync(),
            Port = await _primitivesReader.ReadIntAsync(),
            MainChannel = await _primitivesReader.ReadIntAsync(),
            EffectChannel = await _primitivesReader.ReadIntAsync(),
            FretsCount = await _primitivesReader.ReadIntAsync(),
            CapoPosition = await _primitivesReader.ReadIntAsync(),
            Color = await _primitivesReader.ReadColorAsync(),
            SecondaryFlags = (Gp5Track.Secondary)await _primitivesReader.ReadShortAsync(),
            RseAutoAccentuation = await _primitivesReader.ReadByteAsync(),
            MidiBank = await _primitivesReader.ReadByteAsync(),
            RseHumanPlaying = await _primitivesReader.ReadByteAsync(),
            _A01 = await _primitivesReader.ReadIntAsync(),
            _A02 = await _primitivesReader.ReadIntAsync(),
            _A03 = await _primitivesReader.ReadIntAsync(),
            _B01 = await _primitivesReader.ReadByteAsync(),
            _B02 = await _primitivesReader.ReadByteAsync(),
            _B03 = await _primitivesReader.ReadByteAsync(),
            _B04 = await _primitivesReader.ReadByteAsync(),
            _B05 = await _primitivesReader.ReadByteAsync(),
            _B06 = await _primitivesReader.ReadByteAsync(),
            _B07 = await _primitivesReader.ReadByteAsync(),
            _B08 = await _primitivesReader.ReadByteAsync(),
            _B09 = await _primitivesReader.ReadByteAsync(),
            _B10 = await _primitivesReader.ReadByteAsync(),
            _C01 = await _primitivesReader.ReadShortAsync(),
            RseInstrument = await ReadRseInstrumentAsync(),
            RseEqualizer = await _rseEqualizerReader.ReadRseEqualizerAsync(Gp5Track.RseEqualizerBandsCount),
            RseInstrumentEffect = await ReadRseInstrumentEffectAsync()
        };
    }

    private async ValueTask<int[]> ReadStringsTuningAsync()
    {
        var stringsTuning = new int[Gp5Track.StringsTuningLength];
        for (var i = 0; i < stringsTuning.Length; i++)
        {
            stringsTuning[i] = await _primitivesReader.ReadIntAsync();
        }

        return stringsTuning;
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

    private async ValueTask<Gp5MixTable> ReadMixTableAsync()
    {
        var mixTable = new Gp5MixTable
        {
            Instrument = await _primitivesReader.ReadSignedByteAsync(),
            RseInstrument = await ReadRseInstrumentAsync(),
            Volume = await _primitivesReader.ReadSignedByteAsync(),
            Balance = await _primitivesReader.ReadSignedByteAsync(),
            Chorus = await _primitivesReader.ReadSignedByteAsync(),
            Reverb = await _primitivesReader.ReadSignedByteAsync(),
            Phaser = await _primitivesReader.ReadSignedByteAsync(),
            Tremolo = await _primitivesReader.ReadSignedByteAsync(),
            Tempo = await ReadTempoAsync()
        };

        bool HasValueChange(int value) => value != -1;

        if (HasValueChange(mixTable.Volume))
            mixTable.VolumeTransition = await _primitivesReader.ReadByteAsync();

        if (HasValueChange(mixTable.Balance))
            mixTable.BalanceTransition = await _primitivesReader.ReadByteAsync();

        if (HasValueChange(mixTable.Chorus))
            mixTable.ChorusTransition = await _primitivesReader.ReadByteAsync();

        if (HasValueChange(mixTable.Reverb))
            mixTable.ReverbTransition = await _primitivesReader.ReadByteAsync();

        if (HasValueChange(mixTable.Phaser))
            mixTable.PhaserTransition = await _primitivesReader.ReadByteAsync();

        if (HasValueChange(mixTable.Tremolo))
            mixTable.TremoloTransition = await _primitivesReader.ReadByteAsync();

        if (HasValueChange(mixTable.Tempo.BeatsPerMinute))
        {
            mixTable.TempoTransition = await _primitivesReader.ReadByteAsync();
            mixTable.Tempo.HideBeatsPerMinute = await _primitivesReader.ReadBoolAsync();// TODO: named method via Demetra principle
        }

        mixTable.PrimaryFlags = (Gp5MixTable.Primary)await _primitivesReader.ReadByteAsync();
        mixTable.WahWah = await _primitivesReader.ReadSignedByteAsync();
        mixTable.RseInstrumentEffect = await ReadRseInstrumentEffectAsync();

        return mixTable;
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
