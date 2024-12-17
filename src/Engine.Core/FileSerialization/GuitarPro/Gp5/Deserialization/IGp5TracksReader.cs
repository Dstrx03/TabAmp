﻿using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal interface IGp5TracksReader
{
    ValueTask<Gp5MidiChannel> ReadMidiChannelAsync();
    ValueTask<Gp5Track> ReadTrackAsync();
    ValueTask<Gp5MixTable> ReadMixTableAsync();
    ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync();
    ValueTask<int> ReadRseMasterEffectReverbAsync();
}