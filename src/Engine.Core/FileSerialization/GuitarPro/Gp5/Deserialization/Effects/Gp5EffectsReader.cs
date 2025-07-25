﻿using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.BinaryPrimitives;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Effects;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Effects;

internal class Gp5EffectsReader : IGp5EffectsReader
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;

    public Gp5EffectsReader(IGp5BinaryPrimitivesReader primitivesReader) =>
        _primitivesReader = primitivesReader;

    public async ValueTask<Gp5Bend> ReadBendAsync()
    {
        var bend = new Gp5Bend
        {
            Type = await _primitivesReader.ReadByteAsync(),
            PitchShift = await _primitivesReader.ReadIntAsync(),
            Points = new (int, int, byte)[await _primitivesReader.ReadIntAsync()]
        };

        for (var i = 0; i < bend.Points.Length; i++)
        {
            var timePosition = await _primitivesReader.ReadIntAsync();
            var pitchShift = await _primitivesReader.ReadIntAsync();
            var vibrato = await _primitivesReader.ReadByteAsync();

            bend.Points[i] = (timePosition, pitchShift, vibrato);
        }

        return bend;
    }

    public async ValueTask<Gp5GraceNote> ReadGraceNoteAsync()
    {
        return new Gp5GraceNote
        {
            Fret = await _primitivesReader.ReadByteAsync(),
            Dynamic = await _primitivesReader.ReadByteAsync(),
            Transition = await _primitivesReader.ReadByteAsync(),
            Duration = await _primitivesReader.ReadByteAsync(),
            PrimaryFlags = (Gp5GraceNote.Primary)await _primitivesReader.ReadByteAsync()
        };
    }

    public async ValueTask<Gp5Harmonic> ReadHarmonicAsync()
    {
        var harmonic = new Gp5Harmonic
        {
            Type = await _primitivesReader.ReadByteAsync()
        };

        if (harmonic.Type == 2)
        {
            harmonic.Note = await _primitivesReader.ReadByteAsync();
            harmonic.Accidental = await _primitivesReader.ReadSignedByteAsync();
            harmonic.Octave = await _primitivesReader.ReadByteAsync();
        }
        else if (harmonic.Type == 3)
        {
            harmonic.Fret = await _primitivesReader.ReadByteAsync();
        }

        return harmonic;
    }
}
