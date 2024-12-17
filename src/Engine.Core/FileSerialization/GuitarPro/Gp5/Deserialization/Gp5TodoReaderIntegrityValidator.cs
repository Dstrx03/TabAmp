using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

[Obsolete]
internal class Gp5TodoReaderIntegrityValidator : IGp5TodoReader
{
    private readonly IGp5TodoReader _reader;

    public Gp5TodoReaderIntegrityValidator(IGp5TodoReader reader) =>
        _reader = reader;

    

    

    

    

    

    public ValueTask<Gp5Tempo> ReadHeaderTempoAsync() =>
        _reader.ReadHeaderTempoAsync();

    

    

    

    

    public async ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst)
    {
        var measureHeader = await _reader.ReadMeasureHeaderAsync(isFirst);

        if (measureHeader._A01 != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"measure header _A01 expected to be 0: _A01={measureHeader._A01}");

        if (!measureHeader.PrimaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasAlternateEndings) && measureHeader.AlternateEndingsFlags != 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"AlternateEndingsFlags expected to be 0 due to measure has no alternate endings: AlternateEndingsFlags={measureHeader.AlternateEndingsFlags}");

        return measureHeader;
    }

    

    public ValueTask<byte> ReadMeasureBreakLineAsync() =>
        _reader.ReadMeasureBreakLineAsync();

    public async ValueTask<int> ReadMeasureBeatsCountAsync()
    {
        var beatsCount = await _reader.ReadMeasureBeatsCountAsync();

        if (beatsCount < 1 || beatsCount > 127)
            // TODO: message
            throw new FileSerializationIntegrityException($"beatsCount out of valid range: beatsCount={beatsCount}");

        return beatsCount;
    }

    public ValueTask<Gp5Beat> ReadBeatAsync(Func<Gp5Beat, ValueTask> readNotesAsync) =>
        _reader.ReadBeatAsync(readNotesAsync);

    public ValueTask<Gp5Note> ReadNoteAsync() =>
        _reader.ReadNoteAsync();
}
