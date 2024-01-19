namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5Lyrics
{
    public int ApplyToTrack { get; set; }
    public Gp5LyricsLine FirstLine { get; set; }
    public Gp5LyricsLine SecondLine { get; set; }
    public Gp5LyricsLine ThirdLine { get; set; }
    public Gp5LyricsLine FourthLine { get; set; }
    public Gp5LyricsLine FifthLine { get; set; }
}
