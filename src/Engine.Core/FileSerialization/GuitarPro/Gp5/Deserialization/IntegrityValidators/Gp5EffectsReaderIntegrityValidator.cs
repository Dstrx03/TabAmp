using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.IntegrityValidators;

internal class Gp5EffectsReaderIntegrityValidator : IGp5EffectsReader
{
    private readonly IGp5EffectsReader _effectsReader;

    public Gp5EffectsReaderIntegrityValidator(IGp5EffectsReader effectsReader) =>
        _effectsReader = effectsReader;

    public ValueTask<Gp5Bend> ReadBendAsync() =>
        _effectsReader.ReadBendAsync();

    public ValueTask<Gp5GraceNote> ReadGraceNoteAsync() =>
        _effectsReader.ReadGraceNoteAsync();

    public ValueTask<Gp5Harmonic> ReadHarmonicAsync() =>
        _effectsReader.ReadHarmonicAsync();
}
