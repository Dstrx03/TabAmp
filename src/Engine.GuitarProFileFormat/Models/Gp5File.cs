namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5File
{
    public string Version { get; set; }
    public Gp5ScoreInformation ScoreInformation { get; set; }
    public Gp5Lyrics Lyrics { get; set; }
    public Gp5RseMasterEffect RseMasterEffect { get; set; }
    public Gp5PageSetup PageSetup { get; set; }
    public Gp5Tempo Tempo { get; set; }
    public Gp5HeaderKeySignature KeySignature { get; set; }
    public Gp5MidiChannel[] MidiChannels { get; set; }
}
