using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Readers;

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
        throw new NotImplementedException("TODO: read harmonic.");
    }
}
