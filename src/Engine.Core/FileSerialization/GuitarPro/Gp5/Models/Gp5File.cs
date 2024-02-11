namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5File
{
    public const int VersionStringMaxLength = 30;
    public const int MidiChannelsCount = 64;

    public string Version { get; set; }
    public Gp5ScoreInformation ScoreInformation { get; set; }
    public Gp5Lyrics Lyrics { get; set; }
    public Gp5RseMasterEffect RseMasterEffect { get; set; }
    public Gp5PageSetup PageSetup { get; set; }
    public Gp5Tempo Tempo { get; set; }
    public Gp5HeaderKeySignature KeySignature { get; set; }
    public Gp5MidiChannel[] MidiChannels { get; set; }
    public Gp5MusicalDirections MusicalDirections { get; set; }
    public int MeasuresCount { get; set; }
    public int TracksCount { get; set; }
    public Gp5MeasureHeader[] MeasureHeaders { get; set; }
}
