﻿using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IntegrityValidation;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Measures;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Measures;

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
            throw new ProcessIntegrityException($"measure header _A01 expected to be 0: _A01={measureHeader._A01}");

        if (!measureHeader.PrimaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasAlternateEndings) && measureHeader.AlternateEndingsFlags != 0)
            // TODO: message
            throw new ProcessIntegrityException($"AlternateEndingsFlags expected to be 0 due to measure has no alternate endings: AlternateEndingsFlags={measureHeader.AlternateEndingsFlags}");

        return measureHeader;
    }

    public ValueTask<byte> ReadMeasureBreakLineAsync() =>
        _measuresReader.ReadMeasureBreakLineAsync();

    public async ValueTask<int> ReadMeasureBeatsCountAsync()
    {
        var beatsCount = await _measuresReader.ReadMeasureBeatsCountAsync();

        if (beatsCount < 1 || beatsCount > 127)
            // TODO: message
            throw new ProcessIntegrityException($"beatsCount out of valid range: beatsCount={beatsCount}");

        return beatsCount;
    }

    public ValueTask<Gp5Beat> ReadBeatAsync(Func<Gp5Beat, ValueTask> readNotesAsync) =>
        _measuresReader.ReadBeatAsync(readNotesAsync);

    public async ValueTask<Gp5Note> ReadNoteAsync()
    {
        var note = await _measuresReader.ReadNoteAsync();

        if (!note.PrimaryFlags.HasFlag(Gp5Note.Primary._A01))
            // TODO: message
            throw new ProcessIntegrityException("note expected to have primary flag _A01");

        return note;
    }
}
