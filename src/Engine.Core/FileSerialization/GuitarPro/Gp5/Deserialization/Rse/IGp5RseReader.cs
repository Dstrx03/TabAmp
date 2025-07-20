using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Rse;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Rse;

internal interface IGp5RseReader
{
    ValueTask<Gp5RseInstrument> ReadRseInstrumentAsync();
    ValueTask<Gp5RseInstrumentEffect> ReadRseInstrumentEffectAsync();
    ValueTask<Gp5RseEqualizer> ReadRseEqualizerAsync(int bandsCount);
}
