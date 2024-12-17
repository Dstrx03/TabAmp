using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal interface IGp5EffectsReader
{
    ValueTask<Gp5Bend> ReadBendAsync();
    ValueTask<Gp5GraceNote> ReadGraceNoteAsync();
    ValueTask<Gp5Harmonic> ReadHarmonicAsync();
}
