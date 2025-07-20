using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Rse;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Rse;

internal class Gp5RseReaderIntegrityValidator : IGp5RseReader
{
    private readonly IGp5RseReader _rseReader;

    public Gp5RseReaderIntegrityValidator(IGp5RseReader rseReader) =>
        _rseReader = rseReader;

    public ValueTask<Gp5RseEqualizer> ReadRseEqualizerAsync(int bandsCount) =>
        _rseReader.ReadRseEqualizerAsync(bandsCount);

    public ValueTask<Gp5RseInstrument> ReadRseInstrumentAsync() =>
        _rseReader.ReadRseInstrumentAsync();

    public ValueTask<Gp5RseInstrumentEffect> ReadRseInstrumentEffectAsync() =>
        _rseReader.ReadRseInstrumentEffectAsync();
}
