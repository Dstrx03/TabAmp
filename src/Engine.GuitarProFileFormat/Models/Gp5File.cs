namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5File
{
    public string Version { get; set; }
    public Gp5ScoreInformation ScoreInformation { get; set; }
    public Gp5Lyrics Lyrics { get; set; }
    public Gp5RseMasterEffect RseMasterEffect { get; set; }
}
