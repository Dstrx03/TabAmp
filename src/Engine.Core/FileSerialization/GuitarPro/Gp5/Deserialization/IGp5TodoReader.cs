﻿using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal interface IGp5TodoReader
{
    ValueTask<string> ReadVersionAsync();
    ValueTask<Gp5ScoreInformation> ReadScoreInformationAsync();
    ValueTask<Gp5Lyrics> ReadLyricsAsync();
    ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync();
    ValueTask<Gp5PageSetup> ReadPageSetupAsync();
    ValueTask<Gp5Tempo> ReadHeaderTempoAsync();
    ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync();
    ValueTask<Gp5MidiChannel> ReadMidiChannelAsync();
    ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync();
    ValueTask<int> ReadRseMasterEffectReverbAsync();
    ValueTask<int> ReadMeasuresCountAsync();
    ValueTask<int> ReadTracksCountAsync();
    ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst);
}
