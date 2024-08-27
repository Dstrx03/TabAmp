using System.Threading.Tasks;
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
        await NextMeasuresAndTracksCountAsync();
        await NextMeasureHeadersAsync();
        await NextTracksAsync();
        await NextMeasuresAsync();
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

    protected abstract ValueTask NextMeasuresAndTracksCountAsync();

    private async ValueTask NextMeasureHeadersAsync()
    {
        for (var index = 0; index < File.MeasureHeaders.Length; index++)
        {
            await NextMeasureHeaderAsync(index);
        }
    }

    protected abstract ValueTask NextMeasureHeaderAsync(int index);

    private async ValueTask NextTracksAsync()
    {
        for (var index = 0; index < File.Tracks.Length; index++)
        {
            await NextTrackAsync(index);
        }
    }

    protected abstract ValueTask NextTrackAsync(int index);

    protected virtual async ValueTask NextMeasuresAsync()
    {
        for (var measureIndex = 0; measureIndex < File.Beats.Length; measureIndex++)
        {
            await NextMeasureAsync(measureIndex);
        }
    }

    private async ValueTask NextMeasureAsync(int measureIndex)
    {
        await NextMeasureBeatsCountAsync(measureIndex);
        await NextBeatsAsync(measureIndex);
    }

    protected abstract ValueTask NextMeasureBeatsCountAsync(int measureIndex);

    private async ValueTask NextBeatsAsync(int measureIndex)
    {
        for (var beatIndex = 0; beatIndex < File.Beats[measureIndex]?.Length; beatIndex++)
        {
            await NextBeatAsync(beatIndex: beatIndex, measureIndex: measureIndex);
        }
    }

    protected abstract ValueTask NextBeatAsync(int beatIndex, int measureIndex);
}
