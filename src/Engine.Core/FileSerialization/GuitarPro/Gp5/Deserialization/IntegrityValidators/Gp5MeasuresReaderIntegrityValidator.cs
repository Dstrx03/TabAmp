using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.IntegrityValidators;

internal class Gp5MeasuresReaderIntegrityValidator : IGp5MeasuresReader
{
    private readonly IGp5MeasuresReader _measuresReader;

    public Gp5MeasuresReaderIntegrityValidator(IGp5MeasuresReader measuresReader) =>
        _measuresReader = measuresReader;

    public async ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst)
    {
        var measureHeader = await _measuresReader.ReadMeasureHeaderAsync(isFirst);

        if (measureHeader._A01 != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"measure header _A01 expected to be 0: _A01={measureHeader._A01}");

        if (!measureHeader.PrimaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasAlternateEndings) && measureHeader.AlternateEndingsFlags != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"AlternateEndingsFlags expected to be 0 due to measure has no alternate endings: AlternateEndingsFlags={measureHeader.AlternateEndingsFlags}");

        return measureHeader;
    }

    public ValueTask<byte> ReadMeasureBreakLineAsync() =>
        _measuresReader.ReadMeasureBreakLineAsync();

    public async ValueTask<int> ReadMeasureBeatsCountAsync()
    {
        var beatsCount = await _measuresReader.ReadMeasureBeatsCountAsync();

        if (beatsCount < 1 || beatsCount > 127)
            // TODO: message
            throw new FileSerializationIntegrityException($"beatsCount out of valid range: beatsCount={beatsCount}");

        return beatsCount;
    }

    public ValueTask<Gp5Beat> ReadBeatAsync(Func<Gp5Beat, ValueTask> readNotesAsync) =>
        _measuresReader.ReadBeatAsync(readNotesAsync);

    public ValueTask<Gp5Note> ReadNoteAsync() =>
        _measuresReader.ReadNoteAsync();
}
