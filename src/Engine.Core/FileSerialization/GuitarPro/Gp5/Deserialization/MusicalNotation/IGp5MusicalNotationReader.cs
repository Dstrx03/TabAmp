using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.MusicalNotation;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.MusicalNotation;

internal interface IGp5MusicalNotationReader
{
    ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync();
    ValueTask<Gp5KeySignature> ReadKeySignatureAsync();
    ValueTask<Gp5TimeSignature> ReadTimeSignatureAsync(bool hasDenominator);
    ValueTask<byte[]> ReadTimeSignatureBeamGroupsAsync();
    ValueTask<Gp5Tempo> ReadTempoAsync();
    ValueTask<Gp5Bool> ReadTempoHideBeatsPerMinuteAsync();
    ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync();
    ValueTask<Gp5Chord> ReadChordAsync();
    ValueTask<Gp5BeatEffects> ReadBeatEffectsAsync();
    ValueTask<Gp5NoteEffects> ReadNoteEffectsAsync();
}
