namespace TabAmp.Models;

public class GP5Song
{
    public string Version { get; set; }
    public ScoreInformation ScoreInformation { get; set; }
    public Lyrics Lyrics { get; set; }
    public RSEMasterEffect RSEMasterEffect { get; set; }
    public PageSetup PageSetup { get; set; }
    public Tempo Tempo { get; set; }
    public sbyte Key { get; set; }
    public int Octave { get; set; }
}
