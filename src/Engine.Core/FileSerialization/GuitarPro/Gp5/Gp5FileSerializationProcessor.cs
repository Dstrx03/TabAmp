﻿using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5;

internal abstract class Gp5FileSerializationProcessor : IFileSerializationProcessor
{
    public string SupportedFileExtensions => ".gp5";

    protected Gp5File File { get; private set; }

    protected async Task ProcessAsync(Gp5File? file = null)
    {
        File = file ?? new();

        await NextVersionAsync();
        await NextScoreInformationAsync();
        await NextLyricsAsync();
        await NextRseMasterEffectAsync();
        await NextPageSetupAsync();
        await NextHeaderTempoAsync();
        await NextHeaderKeySignatureAsync();
        await NextMidiChannelsAsync();
        await NextMusicalDirectionsAsync();
        await NextRseMasterEffectReverbAsync();
        await NextMeasuresCountAsync();
        await NextTracksCountAsync();
        await NextMeasureHeadersAsync();
    }

    protected abstract ValueTask NextVersionAsync();

    protected abstract ValueTask NextScoreInformationAsync();

    protected abstract ValueTask NextLyricsAsync();

    protected abstract ValueTask NextRseMasterEffectAsync();

    protected abstract ValueTask NextPageSetupAsync();

    protected abstract ValueTask NextHeaderTempoAsync();

    protected abstract ValueTask NextHeaderKeySignatureAsync();

    protected virtual async ValueTask NextMidiChannelsAsync()
    {
        for (var index = 0; index < File.MidiChannels.Length; index++)
        {
            await NextMidiChannelAsync(index);
        }
    }

    protected abstract ValueTask NextMidiChannelAsync(int index);

    protected abstract ValueTask NextMusicalDirectionsAsync();

    protected abstract ValueTask NextRseMasterEffectReverbAsync();

    protected abstract ValueTask NextMeasuresCountAsync();

    protected abstract ValueTask NextTracksCountAsync();

    protected virtual async ValueTask NextMeasureHeadersAsync()
    {
        for (var index = 0; index < File.MeasureHeaders.Length; index++)
        {
            await NextMeasureHeaderAsync(index);
        }
    }

    protected abstract ValueTask NextMeasureHeaderAsync(int index);
}
