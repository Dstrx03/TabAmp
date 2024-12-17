using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal interface IGp5DocumentComponentsReader
{
    ValueTask<Gp5ByteText> ReadVersionAsync();
    ValueTask<(int measureHeadersCount, int tracksCount)> ReadMeasureHeadersAndTracksCountAsync();
    ValueTask<Gp5ScoreInformation> ReadScoreInformationAsync();
    ValueTask<Gp5PageSetup> ReadPageSetupAsync();
    ValueTask<Gp5Lyrics> ReadLyricsAsync();
    ValueTask<Gp5Marker> ReadMarkerAsync();
}
