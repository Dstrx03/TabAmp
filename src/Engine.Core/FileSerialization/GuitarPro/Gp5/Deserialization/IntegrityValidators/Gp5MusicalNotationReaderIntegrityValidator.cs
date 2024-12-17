using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.IntegrityValidators;

internal class Gp5MusicalNotationReaderIntegrityValidator : IGp5MusicalNotationReader
{
    private readonly IGp5MusicalNotationReader _notationReader;

    public Gp5MusicalNotationReaderIntegrityValidator(IGp5MusicalNotationReader notationReader) =>
        _notationReader = notationReader;

    public async ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync()
    {
        var keySignature = await _notationReader.ReadHeaderKeySignatureAsync();

        if (keySignature.Octave != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"expected to be 0: Octave={keySignature.Octave}");

        if (keySignature.Key >= 0)
        {
            if (keySignature._A01 != 0 || keySignature._A02 != 0 || keySignature._A03 != 0)
                // TODO: message
                throw new FileSerializationIntegrityException($"expected to be 0: _A01={keySignature._A01}, _A02={keySignature._A02}, _A03={keySignature._A03}, ");
        }
        else
        {
            if (keySignature._A01 != -1 || keySignature._A02 != -1 || keySignature._A03 != -1)
                // TODO: message
                throw new FileSerializationIntegrityException($"expected to be -1: _A01={keySignature._A01}, _A02={keySignature._A02}, _A03={keySignature._A03}, ");
        }

        return keySignature;
    }

    public ValueTask<Gp5KeySignature> ReadKeySignatureAsync() =>
        _notationReader.ReadKeySignatureAsync();

    public ValueTask<Gp5TimeSignature> ReadTimeSignatureAsync(bool hasDenominator) =>
        _notationReader.ReadTimeSignatureAsync(hasDenominator);

    public ValueTask<Gp5TimeSignatureBeamGroups> ReadTimeSignatureBeamGroupsAsync() =>
        _notationReader.ReadTimeSignatureBeamGroupsAsync();

    public ValueTask<Gp5Tempo> ReadTempoAsync() =>
        _notationReader.ReadTempoAsync();

    public ValueTask<Gp5Bool> ReadTempoHideBeatsPerMinuteAsync() =>
        _notationReader.ReadTempoHideBeatsPerMinuteAsync();

    public ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync() =>
        _notationReader.ReadMusicalDirectionsAsync();

    public ValueTask<Gp5Chord> ReadChordAsync() =>
        _notationReader.ReadChordAsync();

    public ValueTask<Gp5BeatEffects> ReadBeatEffectsAsync() =>
        _notationReader.ReadBeatEffectsAsync();

    public ValueTask<Gp5NoteEffects> ReadNoteEffectsAsync() =>
        _notationReader.ReadNoteEffectsAsync();
}
