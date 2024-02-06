using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5;

internal abstract class Gp5FileSerializationProcessor : IFileSerializationProcessor
{
    protected Gp5File _file;

    public string SupportedFileExtensions => ".gp5";

    protected async Task ProcessAsync()
    {
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

    protected abstract ValueTask NextMidiChannelsAsync();

    protected abstract ValueTask NextMusicalDirectionsAsync();

    protected abstract ValueTask NextRseMasterEffectReverbAsync();

    protected abstract ValueTask NextMeasuresCountAsync();

    protected abstract ValueTask NextTracksCountAsync();

    protected abstract ValueTask NextMeasureHeadersAsync();
}
