﻿using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

[Obsolete]
internal interface IGp5TodoReader
{
    
    
    
    ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync();
    
    ValueTask<Gp5Tempo> ReadHeaderTempoAsync();
    
    ValueTask<Gp5MidiChannel> ReadMidiChannelAsync();
    
    ValueTask<int> ReadRseMasterEffectReverbAsync();
    
    ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst);
    ValueTask<Gp5Track> ReadTrackAsync();
    ValueTask<byte> ReadMeasureBreakLineAsync();
    ValueTask<int> ReadMeasureBeatsCountAsync();
    ValueTask<Gp5Beat> ReadBeatAsync(Func<Gp5Beat, ValueTask> readNotesAsync);
    ValueTask<Gp5Note> ReadNoteAsync();
}
