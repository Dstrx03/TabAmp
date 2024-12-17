﻿using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal interface IGp5MusicalNotationReader
{
    ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync();
    ValueTask<Gp5KeySignature> ReadKeySignatureAsync();
    ValueTask<Gp5TimeSignature> ReadTimeSignatureAsync(bool hasDenominator);
    ValueTask<Gp5TimeSignatureBeamGroups> ReadTimeSignatureBeamGroupsAsync();
    ValueTask<Gp5Tempo> ReadTempoAsync();
    ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync();
    ValueTask<Gp5Chord> ReadChordAsync();
    ValueTask<Gp5BeatEffects> ReadBeatEffectsAsync();
    ValueTask<Gp5NoteEffects> ReadNoteEffectsAsync();
}
