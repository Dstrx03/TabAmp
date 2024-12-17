using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal interface IGp5MeasuresReader
{
    ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst);
    ValueTask<byte> ReadMeasureBreakLineAsync();
    ValueTask<int> ReadMeasureBeatsCountAsync();
    ValueTask<Gp5Beat> ReadBeatAsync(Func<Gp5Beat, ValueTask> readNotesAsync);
    ValueTask<Gp5Note> ReadNoteAsync();
}
