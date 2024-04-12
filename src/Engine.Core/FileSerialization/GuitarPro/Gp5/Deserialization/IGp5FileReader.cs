using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal interface IGp5FileReader
{
    ValueTask<byte> ReadByteAsync();
    ValueTask<sbyte> ReadSignedByteAsync();
    ValueTask<bool> ReadBoolAsync();
    ValueTask<short> ReadShortAsync();
    ValueTask<int> ReadIntAsync();
    ValueTask<float> ReadFloatAsync();
    ValueTask<double> ReadDoubleAsync();

    ValueTask<string> ReadStringAsync(int length);
    ValueTask<string> ReadByteStringAsync(int maxLength);
    ValueTask<string> ReadIntStringAsync();
    ValueTask<string> ReadIntByteStringAsync();

    ValueTask<string> ReadVersionAsync();
    ValueTask<Gp5ScoreInformation> ReadScoreInformationAsync();
    ValueTask<Gp5Lyrics> ReadLyricsAsync();
    ValueTask<Gp5LyricsLine> ReadLyricsLineAsync();
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
    ValueTask<Gp5KeySignature> ReadKeySignatureAsync();
    ValueTask<Gp5TimeSignature> ReadTimeSignatureAsync(bool hasNumerator, bool hasDenominator);
    ValueTask<Gp5TimeSignatureBeamGroups> ReadTimeSignatureBeamGroupsAsync();
    ValueTask<Gp5Marker> ReadMarkerAsync();
    ValueTask<Gp5RseEqualizer> ReadRseEqualizerAsync(int bandsCount);
    ValueTask<Gp5Color> ReadColorAsync();
}
