using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.MusicalNotation;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Readers;

internal class Gp5MusicalNotationReader : IGp5MusicalNotationReader
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;
    private readonly IGp5TextReader _textReader;
    private readonly IGp5EffectsReader _effectsReader;

    public Gp5MusicalNotationReader(IGp5BinaryPrimitivesReader primitivesReader, IGp5TextReader textReader,
        IGp5EffectsReader effectsReader) =>
        (_primitivesReader, _textReader, _effectsReader) = (primitivesReader, textReader, effectsReader);

    public async ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync()
    {
        return new Gp5HeaderKeySignature
        {
            Key = await _primitivesReader.ReadSignedByteAsync(),
            _A01 = await _primitivesReader.ReadSignedByteAsync(),
            _A02 = await _primitivesReader.ReadSignedByteAsync(),
            _A03 = await _primitivesReader.ReadSignedByteAsync(),
            Octave = await _primitivesReader.ReadSignedByteAsync()
        };
    }

    public async ValueTask<Gp5KeySignature> ReadKeySignatureAsync()
    {
        return new Gp5KeySignature
        {
            Key = await _primitivesReader.ReadSignedByteAsync(),
            IsMinorKey = await _primitivesReader.ReadBoolAsync()
        };
    }

    public async ValueTask<Gp5TimeSignature> ReadTimeSignatureAsync(bool hasDenominator)
    {
        return new Gp5TimeSignature
        {
            Numerator = await _primitivesReader.ReadByteAsync(),
            Denominator = hasDenominator ? await _primitivesReader.ReadByteAsync() : null
        };
    }

    public async ValueTask<byte[]> ReadTimeSignatureBeamGroupsAsync()
    {
        var beamGroups = new byte[Gp5TimeSignature.BeamGroupsLength];
        for (var i = 0; i < beamGroups.Length; i++)
            beamGroups[i] = await _primitivesReader.ReadByteAsync();

        return beamGroups;
    }

    public async ValueTask<Gp5Tempo> ReadTempoAsync()
    {
        return new Gp5Tempo
        {
            WordIndication = await _textReader.ReadIntByteTextAsync(),
            BeatsPerMinute = await _primitivesReader.ReadIntAsync()
        };
    }

    public ValueTask<Gp5Bool> ReadTempoHideBeatsPerMinuteAsync() =>
        _primitivesReader.ReadBoolAsync();

    public async ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync()
    {
        return new Gp5MusicalDirections
        {
            Coda = await _primitivesReader.ReadShortAsync(),
            DoubleCoda = await _primitivesReader.ReadShortAsync(),
            Segno = await _primitivesReader.ReadShortAsync(),
            SegnoSegno = await _primitivesReader.ReadShortAsync(),
            Fine = await _primitivesReader.ReadShortAsync(),
            DaCapo = await _primitivesReader.ReadShortAsync(),
            DaCapoAlCoda = await _primitivesReader.ReadShortAsync(),
            DaCapoAlDoubleCoda = await _primitivesReader.ReadShortAsync(),
            DaCapoAlFine = await _primitivesReader.ReadShortAsync(),
            DaSegno = await _primitivesReader.ReadShortAsync(),
            DaSegnoAlCoda = await _primitivesReader.ReadShortAsync(),
            DaSegnoAlDoubleCoda = await _primitivesReader.ReadShortAsync(),
            DaSegnoAlFine = await _primitivesReader.ReadShortAsync(),
            DaSegnoSegno = await _primitivesReader.ReadShortAsync(),
            DaSegnoSegnoAlCoda = await _primitivesReader.ReadShortAsync(),
            DaSegnoSegnoAlDoubleCoda = await _primitivesReader.ReadShortAsync(),
            DaSegnoSegnoAlFine = await _primitivesReader.ReadShortAsync(),
            DaCoda = await _primitivesReader.ReadShortAsync(),
            DaDoubleCoda = await _primitivesReader.ReadShortAsync()
        };
    }

    public async ValueTask<Gp5Chord> ReadChordAsync()
    {
        throw new NotImplementedException("TODO: complete chord reading.");

        var isNewFormat = await _primitivesReader.ReadBoolAsync();
        if (!isNewFormat)
            throw new InvalidOperationException("Expected chord to have ~new~ format.");

        var chord = new Gp5Chord
        {
            Sharp = await _primitivesReader.ReadBoolAsync(),
            _A01 = await _primitivesReader.ReadByteAsync(),
            _A02 = await _primitivesReader.ReadByteAsync(),
            _A03 = await _primitivesReader.ReadByteAsync(),
            Root = await _primitivesReader.ReadByteAsync(),
            Type = await _primitivesReader.ReadByteAsync(),
            Extension = await _primitivesReader.ReadByteAsync(),
            Bass = await _primitivesReader.ReadIntAsync(),
            Tonality = await _primitivesReader.ReadIntAsync(),
            Add = await _primitivesReader.ReadBoolAsync(),
            Name = await _textReader.ReadByteTextAsync(22),
            FifthTonality = await _primitivesReader.ReadByteAsync(),
            NinthTonality = await _primitivesReader.ReadByteAsync(),
            EleventhTonality = await _primitivesReader.ReadByteAsync(),
            Fret = await _primitivesReader.ReadIntAsync()
        };

        var frets = new int[7];
        chord.Frets = frets;
        for (var i = 0; i < frets.Length; i++)
            frets[i] = await _primitivesReader.ReadIntAsync();

        chord.BarresCount = await _primitivesReader.ReadByteAsync();

        var barreFrets = new byte[5];
        chord.BarreFrets = barreFrets;
        for (var i = 0; i < barreFrets.Length; i++)
            barreFrets[i] = await _primitivesReader.ReadByteAsync();

        var barreStarts = new byte[5];
        chord.BarreStarts = barreStarts;
        for (var i = 0; i < barreStarts.Length; i++)
            barreStarts[i] = await _primitivesReader.ReadByteAsync();

        var barreEnds = new byte[5];
        chord.BarreEnds = barreEnds;
        for (var i = 0; i < barreEnds.Length; i++)
            barreEnds[i] = await _primitivesReader.ReadByteAsync();

        var omissions = new bool[7];
        chord.Omissions = omissions;
        for (var i = 0; i < omissions.Length; i++)
            omissions[i] = await _primitivesReader.ReadBoolAsync();

        chord._B01 = await _primitivesReader.ReadByteAsync();

        var fingerings = new sbyte[7];
        chord.Fingerings = fingerings;
        for (var i = 0; i < fingerings.Length; i++)
            fingerings[i] = await _primitivesReader.ReadSignedByteAsync();

        chord.Show = await _primitivesReader.ReadBoolAsync();

        return chord;
    }

    public async ValueTask<Gp5BeatEffects> ReadBeatEffectsAsync()
    {
        var primaryFlags = (Gp5BeatEffects.Primary)await _primitivesReader.ReadByteAsync();
        var secondaryFlags = (Gp5BeatEffects.Secondary)await _primitivesReader.ReadByteAsync();
        var beatEffects = new Gp5BeatEffects
        {
            PrimaryFlags = primaryFlags,
            SecondaryFlags = secondaryFlags
        };

        if (primaryFlags.HasFlag(Gp5BeatEffects.Primary.HasTappingSlappingPopping))
            beatEffects.TappingSlappingPopping = await _primitivesReader.ReadByteAsync();

        if (secondaryFlags.HasFlag(Gp5BeatEffects.Secondary.HasTremoloBar))
            beatEffects.TremoloBar = await _effectsReader.ReadBendAsync();

        if (primaryFlags.HasFlag(Gp5BeatEffects.Primary.HasStroke))
        {
            beatEffects.UpstrokeDuration = await _primitivesReader.ReadByteAsync();
            beatEffects.DownstrokeDuration = await _primitivesReader.ReadByteAsync();
        }

        if (secondaryFlags.HasFlag(Gp5BeatEffects.Secondary.HasPickStroke))
            beatEffects.PickStroke = await _primitivesReader.ReadByteAsync();

        return beatEffects;
    }

    public async ValueTask<Gp5NoteEffects> ReadNoteEffectsAsync()
    {
        // TODO: test flags
        var primaryFlags = (Gp5NoteEffects.Primary)await _primitivesReader.ReadByteAsync();
        var secondaryFlags = (Gp5NoteEffects.Secondary)await _primitivesReader.ReadByteAsync();
        var noteEffects = new Gp5NoteEffects
        {
            PrimaryFlags = primaryFlags,
            SecondaryFlags = secondaryFlags
        };

        if (primaryFlags.HasFlag(Gp5NoteEffects.Primary.HasBend))
            noteEffects.Bend = await _effectsReader.ReadBendAsync();

        if (primaryFlags.HasFlag(Gp5NoteEffects.Primary.HasGraceNote))
            noteEffects.GraceNote = await _effectsReader.ReadGraceNoteAsync();

        if (secondaryFlags.HasFlag(Gp5NoteEffects.Secondary.HasTremoloPicking))
            noteEffects.TremoloPicking = await _primitivesReader.ReadByteAsync();

        if (secondaryFlags.HasFlag(Gp5NoteEffects.Secondary.HasSlide))
            noteEffects.SlideFlags = (Gp5NoteEffects.Slide)await _primitivesReader.ReadByteAsync();

        if (secondaryFlags.HasFlag(Gp5NoteEffects.Secondary.HasHarmonic))
            noteEffects.Harmonic = await _effectsReader.ReadHarmonicAsync();

        if (secondaryFlags.HasFlag(Gp5NoteEffects.Secondary.HasTrill))
        {
            noteEffects.TrillFret = await _primitivesReader.ReadByteAsync();
            noteEffects.TrillPeriod = await _primitivesReader.ReadByteAsync();
        }

        return noteEffects;
    }
}
