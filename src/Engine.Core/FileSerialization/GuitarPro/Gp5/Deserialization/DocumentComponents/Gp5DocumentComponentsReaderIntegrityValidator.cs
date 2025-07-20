using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IntegrityValidation;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.DocumentComponents;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.DocumentComponents;

internal class Gp5DocumentComponentsReaderIntegrityValidator : IGp5DocumentComponentsReader
{
    private readonly IGp5DocumentComponentsReader _documentReader;

    public Gp5DocumentComponentsReaderIntegrityValidator(IGp5DocumentComponentsReader documentReader) =>
        _documentReader = documentReader;

    public ValueTask<Gp5ByteText> ReadVersionAsync() =>
        _documentReader.ReadVersionAsync();

    public async ValueTask<(int measureHeadersCount, int tracksCount)> ReadMeasureHeadersAndTracksCountAsync()
    {
        var (measureHeadersCount, tracksCount) = await _documentReader.ReadMeasureHeadersAndTracksCountAsync();

        if (measureHeadersCount < 1 || measureHeadersCount > 2048)
            // TODO: message
            throw new ProcessIntegrityException($"measureHeadersCount out of valid range: measureHeadersCount={measureHeadersCount}");

        if (tracksCount < 1 || tracksCount > 127)
            // TODO: message
            throw new ProcessIntegrityException($"tracksCount out of valid range: tracksCount={tracksCount}");

        return (measureHeadersCount, tracksCount);
    }

    public ValueTask<Gp5ScoreInformation> ReadScoreInformationAsync() =>
        _documentReader.ReadScoreInformationAsync();

    public ValueTask<Gp5PageSetup> ReadPageSetupAsync() =>
        _documentReader.ReadPageSetupAsync();

    public ValueTask<Gp5Lyrics> ReadLyricsAsync() =>
        _documentReader.ReadLyricsAsync();

    public ValueTask<Gp5Marker> ReadMarkerAsync() =>
        _documentReader.ReadMarkerAsync();
}
